
using EWallet.Core.Enums;
using EWallet.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EWallet.Core.Seeding;

public static class TransactionSeedData
{
    public static void Seed(DbContext context)
    {
        var wallets = GetWallets(context);

        var senderWallet = wallets[0];
        var receiverWallet = wallets[1];

        var (bankTransfer, qris) = GetPaymentMethods(context);

        var now = DateTime.UtcNow.AddHours(7);

        // Transactions
        var existingTransactions = context.Set<Transactions>()
            .Where(t => t.ReferenceNo.StartsWith("SEED-"))
            .ToList();

        var isFirstSeed = existingTransactions.Count == 0;

        var transactions = CreateTransactions(
            senderWallet,
            receiverWallet,
            now);

        var existingReferences = existingTransactions
            .Select(t => t.ReferenceNo)
            .ToHashSet();

        var newTransactions = transactions
            .Where(t => !existingReferences.Contains(t.ReferenceNo))
            .ToList();

        context.Set<Transactions>().AddRange(newTransactions);

        context.SaveChanges();

        var seededTransactions = context.Set<Transactions>()
            .Where(t => t.ReferenceNo.StartsWith("SEED-"))
            .ToArray();

        // TopUpRequests
        var topUpRequests = CreateTopUpRequests(
            seededTransactions,
            senderWallet,
            bankTransfer,
            qris,
            now);

        var newTopUpRequests = topUpRequests
            .Where(t => !context.Set<TopUpRequests>()
                .Any(existing => existing.TransactionId == t.TransactionId))
            .ToList();

        context.Set<TopUpRequests>().AddRange(newTopUpRequests);

        // WalletMutations
        var mutations = CreateMutations(
            seededTransactions,
            senderWallet,
            receiverWallet);

        var newMutations = mutations
            .Where(m => !context.Set<WalletMutations>().Any(existing =>
                existing.TransactionId == m.TransactionId &&
                existing.WalletId == m.WalletId &&
                existing.Direction == m.Direction))
            .ToList();

        context.Set<WalletMutations>().AddRange(newMutations);

        // AuditLogs
        var auditLogs = CreateAuditLogs(
            seededTransactions,
            senderWallet);

        var newAuditLogs = auditLogs
            .Where(a => !context.Set<AuditLogs>()
                .Any(existing => existing.CorrelationId == a.CorrelationId))
            .ToList();

        context.Set<AuditLogs>().AddRange(newAuditLogs);

        if (isFirstSeed)
        {
            UpdateWalletBalances(
                context,
                senderWallet,
                receiverWallet,
                now);
        }

        context.SaveChanges();
    }

    public static async Task SeedAsync(DbContext context, CancellationToken cancellationToken)
    {
        var wallets = await GetWalletsAsync(
            context,
            cancellationToken);

        var senderWallet = wallets[0];
        var receiverWallet = wallets[1];

        var (bankTransfer, qris) = await GetPaymentMethodsAsync(
            context,
            cancellationToken);

        var now = DateTime.UtcNow.AddHours(7);

        // Transactions
        var existingTransactions = await context.Set<Transactions>()
            .Where(t => t.ReferenceNo.StartsWith("SEED-"))
            .ToListAsync(cancellationToken);

        var isFirstSeed = existingTransactions.Count == 0;

        var transactions = CreateTransactions(
            senderWallet,
            receiverWallet,
            now);

        var existingReferences = existingTransactions
            .Select(t => t.ReferenceNo)
            .ToHashSet();

        var newTransactions = transactions
            .Where(t => !existingReferences.Contains(t.ReferenceNo))
            .ToList();

        await context.Set<Transactions>()
            .AddRangeAsync(newTransactions, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        var seededTransactions = await context.Set<Transactions>()
            .Where(t => t.ReferenceNo.StartsWith("SEED-"))
            .ToArrayAsync(cancellationToken);

        // TopUpRequests
        var topUpRequests = CreateTopUpRequests(
            seededTransactions,
            senderWallet,
            bankTransfer,
            qris,
            now);

        var newTopUpRequests = new List<TopUpRequests>();

        foreach (var topUp in topUpRequests)
        {
            var exists = await context.Set<TopUpRequests>()
                .AnyAsync(
                    existing => existing.TransactionId == topUp.TransactionId,
                    cancellationToken);

            if (!exists)
                newTopUpRequests.Add(topUp);
        }

        await context.Set<TopUpRequests>()
            .AddRangeAsync(newTopUpRequests, cancellationToken);

        // WalletMutations
        var mutations = CreateMutations(
            seededTransactions,
            senderWallet,
            receiverWallet);

        var newMutations = new List<WalletMutations>();

        foreach (var mutation in mutations)
        {
            var exists = await context.Set<WalletMutations>()
                .AnyAsync(existing =>
                    existing.TransactionId == mutation.TransactionId &&
                    existing.WalletId == mutation.WalletId &&
                    existing.Direction == mutation.Direction,
                    cancellationToken);

            if (!exists)
                newMutations.Add(mutation);
        }

        await context.Set<WalletMutations>()
            .AddRangeAsync(newMutations, cancellationToken);

        // AuditLogs
        var auditLogs = CreateAuditLogs(
            seededTransactions,
            senderWallet);

        var newAuditLogs = new List<AuditLogs>();

        foreach (var auditLog in auditLogs)
        {
            var exists = await context.Set<AuditLogs>()
                .AnyAsync(
                    existing => existing.CorrelationId == auditLog.CorrelationId,
                    cancellationToken);

            if (!exists)
                newAuditLogs.Add(auditLog);
        }

        await context.Set<AuditLogs>()
            .AddRangeAsync(newAuditLogs, cancellationToken);

        if (isFirstSeed)
        {
            UpdateWalletBalances(
                context,
                senderWallet,
                receiverWallet,
                now);
        }

        await context.SaveChangesAsync(cancellationToken);
    }


    // METHODS
    private static List<Wallets> GetWallets(DbContext context)
    {
        var wallets = context.Set<Wallets>()
            .Where(w => w.Status == WalletStatus.ACTIVE)
            .OrderBy(w => w.Id)
            .Take(2)
            .ToList();

        ValidateWallets(wallets);

        return wallets;
    }

    private static async Task<List<Wallets>> GetWalletsAsync(
        DbContext context,
        CancellationToken cancellationToken)
    {
        var wallets = await context.Set<Wallets>()
            .Where(w => w.Status == WalletStatus.ACTIVE)
            .OrderBy(w => w.Id)
            .Take(2)
            .ToListAsync(cancellationToken);

        ValidateWallets(wallets);

        return wallets;
    }

    private static void ValidateWallets(List<Wallets> wallets)
    {
        if (wallets.Count < 2)
            throw new InvalidOperationException(
                "At least two active wallets are required.");
    }

    private static (
        PaymentMethods BankTransfer,
        PaymentMethods Qris
    ) GetPaymentMethods(DbContext context)
    {
        var bankTransfer = context.Set<PaymentMethods>()
            .FirstOrDefault(pm =>
                pm.Code == PaymentMethodCode.BANK_TRANSFER &&
                pm.IsActive);

        var qris = context.Set<PaymentMethods>()
            .FirstOrDefault(pm =>
                pm.Code == PaymentMethodCode.QRIS &&
                pm.IsActive);

        if (bankTransfer is null || qris is null)
            throw new InvalidOperationException(
                "Required payment methods were not found.");

        return (bankTransfer, qris);
    }

    private static async Task<(
        PaymentMethods BankTransfer,
        PaymentMethods Qris
    )> GetPaymentMethodsAsync(
        DbContext context,
        CancellationToken cancellationToken)
    {
        var bankTransfer = await context.Set<PaymentMethods>()
            .FirstOrDefaultAsync(
                pm => pm.Code == PaymentMethodCode.BANK_TRANSFER &&
                      pm.IsActive,
                cancellationToken);

        var qris = await context.Set<PaymentMethods>()
            .FirstOrDefaultAsync(
                pm => pm.Code == PaymentMethodCode.QRIS &&
                      pm.IsActive,
                cancellationToken);

        if (bankTransfer is null || qris is null)
            throw new InvalidOperationException(
                "Required payment methods were not found.");

        return (bankTransfer, qris);
    }

    private static Transactions[] CreateTransactions(
        Wallets senderWallet,
        Wallets receiverWallet,
        DateTime now)
    {
        return
        [
            new Transactions(
                "SEED-TOPUP-001",
                senderWallet.Id,
                500000m,
                TransactionType.TOPUP,
                status: TransactionStatus.SUCCESS,
                description: "Dummy successful top up",
                completedAtUtc: now
            ),

            new Transactions(
                "SEED-TOPUP-002",
                senderWallet.Id,
                100000m,
                TransactionType.TOPUP,
                status: TransactionStatus.PENDING,
                description: "Dummy pending top up"
            ),

            new Transactions(
                "SEED-TOPUP-003",
                senderWallet.Id,
                50000m,
                TransactionType.TOPUP,
                status: TransactionStatus.FAILED,
                description: "Dummy failed top up",
                completedAtUtc: now
            ),

            new Transactions(
                "SEED-TRANSFER-001",
                receiverWallet.Id,
                75000m,
                TransactionType.TRANSFER,
                senderWalletId: senderWallet.Id,
                status: TransactionStatus.SUCCESS,
                description: "Dummy successful transfer",
                completedAtUtc: now
            ),

            new Transactions(
                "SEED-TRANSFER-002",
                receiverWallet.Id,
                25000m,
                TransactionType.TRANSFER,
                senderWalletId: senderWallet.Id,
                status: TransactionStatus.FAILED,
                description: "Dummy failed transfer",
                completedAtUtc: now
            )
        ];
    }

    private static TopUpRequests[] CreateTopUpRequests(
        Transactions[] transactions,
        Wallets senderWallet,
        PaymentMethods bankTransfer,
        PaymentMethods qris,
        DateTime now)
    {
        var transactionByReference = transactions
            .ToDictionary(t => t.ReferenceNo);

        var topUpSuccess = transactionByReference["SEED-TOPUP-001"];
        var topUpPending = transactionByReference["SEED-TOPUP-002"];
        var topUpFailed = transactionByReference["SEED-TOPUP-003"];

        return
        [
            new TopUpRequests(
                topUpSuccess.Id,
                senderWallet.Id,
                bankTransfer.Id,
                500000m,
                TopUpRequestStatus.PAID,
                "SEED-PAY-001",
                null,
                now
            ),

            new TopUpRequests(
                topUpPending.Id,
                senderWallet.Id,
                qris.Id,
                100000m,
                TopUpRequestStatus.PENDING,
                "SEED-PAY-002",
                now.AddHours(24)
            ),

            new TopUpRequests(
                topUpFailed.Id,
                senderWallet.Id,
                bankTransfer.Id,
                50000m,
                TopUpRequestStatus.FAILED,
                "SEED-PAY-003"
            )
        ];
    }

    private static WalletMutations[] CreateMutations(
        Transactions[] transactions,
        Wallets senderWallet,
        Wallets receiverWallet)
    {
        var transactionByReference = transactions
            .ToDictionary(t => t.ReferenceNo);

        var topUpSuccess = transactionByReference["SEED-TOPUP-001"];
        var transferSuccess = transactionByReference["SEED-TRANSFER-001"];

        return
        [
            new WalletMutations(
                topUpSuccess.Id,
                senderWallet.Id,
                500000m,
                0m,
                500000m,
                WalletMutationDirection.IN
            ),

            new WalletMutations(
                transferSuccess.Id,
                senderWallet.Id,
                75000m,
                500000m,
                425000m,
                WalletMutationDirection.OUT
            ),

            new WalletMutations(
                transferSuccess.Id,
                receiverWallet.Id,
                75000m,
                0m,
                75000m,
                WalletMutationDirection.IN
            )
        ];
    }

    private static AuditLogs[] CreateAuditLogs(
        Transactions[] transactions,
        Wallets senderWallet)
    {
        var transactionByReference = transactions
            .ToDictionary(t => t.ReferenceNo);

        var topUpSuccess = transactionByReference["SEED-TOPUP-001"];
        var topUpPending = transactionByReference["SEED-TOPUP-002"];
        var topUpFailed = transactionByReference["SEED-TOPUP-003"];
        var transferSuccess = transactionByReference["SEED-TRANSFER-001"];
        var transferFailed = transactionByReference["SEED-TRANSFER-002"];

        return
        [
            new AuditLogs(
                senderWallet.UserId,
                topUpSuccess.Id,
                AuditLogAction.TOPUP_PAID,
                AuditLogEntityType.TRANSACTION,
                topUpSuccess.Id.ToString(),
                AuditLogOutcome.SUCCESS,
                "Top up completed successfully",
                topUpSuccess.ReferenceNo
            ),

            new AuditLogs(
                senderWallet.UserId,
                topUpPending.Id,
                AuditLogAction.TOPUP_PAID,
                AuditLogEntityType.TRANSACTION,
                topUpPending.Id.ToString(),
                AuditLogOutcome.SUCCESS,
                "Top up request created and awaiting payment",
                topUpPending.ReferenceNo
            ),

            new AuditLogs(
                senderWallet.UserId,
                topUpFailed.Id,
                AuditLogAction.TOPUP_REQUESTED,
                AuditLogEntityType.TRANSACTION,
                topUpFailed.Id.ToString(),
                AuditLogOutcome.FAILED,
                "Top up transaction failed",
                topUpFailed.ReferenceNo
            ),

            new AuditLogs(
                senderWallet.UserId,
                transferSuccess.Id,
                AuditLogAction.TRANSFER_CREATED,
                AuditLogEntityType.TRANSACTION,
                transferSuccess.Id.ToString(),
                AuditLogOutcome.SUCCESS,
                "Transfer completed successfully",
                transferSuccess.ReferenceNo
            ),

            new AuditLogs(
                senderWallet.UserId,
                transferFailed.Id,
                AuditLogAction.TRANSFER_REJECTED,
                AuditLogEntityType.TRANSACTION,
                transferFailed.Id.ToString(),
                AuditLogOutcome.FAILED,
                "Transfer transaction failed",
                transferFailed.ReferenceNo
            )
        ];
    }

    private static void UpdateWalletBalances(
        DbContext context,
        Wallets senderWallet,
        Wallets receiverWallet,
        DateTime now)
    {
        context.Entry(senderWallet)
            .Property(w => w.Balance)
            .CurrentValue = 425000m;

        context.Entry(senderWallet)
            .Property(w => w.UpdatedAtUtc)
            .CurrentValue = now;

        context.Entry(receiverWallet)
            .Property(w => w.Balance)
            .CurrentValue = 75000m;

        context.Entry(receiverWallet)
            .Property(w => w.UpdatedAtUtc)
            .CurrentValue = now;
    }
}
