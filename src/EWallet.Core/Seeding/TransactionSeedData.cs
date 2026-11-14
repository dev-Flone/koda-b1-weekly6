
using EWallet.Core.Enums;
using EWallet.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EWallet.Core.Seeding;

public static class TransactionSeedData
{
    public static void Seed(DbContext context)
    {
        bool alreadySeeded = context.Set<Transactions>()
            .Any(t => t.ReferenceNo.StartsWith("SEED-"));

        if (alreadySeeded) return;

        var wallets = context.Set<Wallets>()
            .Where(w => w.Status == WalletStatus.ACTIVE)
            .OrderBy(w => w.Id)
            .Take(2)
            .ToList();

        if (wallets.Count < 2)
            throw new InvalidOperationException(
                "At least two active wallets are required to begin transactions.");

        var senderWallet = wallets[0];
        var receiverWallet = wallets[1];

        var bankTransfer = context.Set<PaymentMethods>()
            .FirstOrDefault(pm =>
                pm.Code == PaymentMethodCode.BANK_TRANSFER && pm.IsActive);

        var qris = context.Set<PaymentMethods>()
            .FirstOrDefault(pm =>
                pm.Code == PaymentMethodCode.QRIS && pm.IsActive);

        if (bankTransfer is null || qris is null)
            throw new InvalidOperationException(
                "Required active payment methods were not found.");

        var now = DateTime.UtcNow.AddHours(7);

        // 1. Create transactions.
        var topUpSuccess = new Transactions(
            "SEED-TOPUP-001",
            senderWallet.Id,
            500000m,
            TransactionType.TOPUP,
            status: TransactionStatus.SUCCESS,
            description: "Dummy successful top up",
            completedAtUtc: now
        );

        var topUpPending = new Transactions(
            "SEED-TOPUP-002",
            senderWallet.Id,
            100000m,
            TransactionType.TOPUP,
            status: TransactionStatus.PENDING,
            description: "Dummy pending top up"
        );

        var topUpFailed = new Transactions(
            "SEED-TOPUP-003",
            senderWallet.Id,
            50000m,
            TransactionType.TOPUP,
            status: TransactionStatus.FAILED,
            description: "Dummy failed top up",
            completedAtUtc: now
        );

        var transferSuccess = new Transactions(
            "SEED-TRANSFER-001",
            receiverWallet.Id,
            75000m,
            TransactionType.TRANSFER,
            senderWalletId: senderWallet.Id,
            status: TransactionStatus.SUCCESS,
            description: "Dummy successful transfer",
            completedAtUtc: now
        );

        var transferFailed = new Transactions(
            "SEED-TRANSFER-002",
            receiverWallet.Id,
            25000m,
            TransactionType.TRANSFER,
            senderWalletId: senderWallet.Id,
            status: TransactionStatus.FAILED,
            description: "Dummy failed transfer",
            completedAtUtc: now
        );

        var transactions = new[]
        {
            topUpSuccess,
            topUpPending,
            topUpFailed,
            transferSuccess,
            transferFailed
        };

        context.Set<Transactions>().AddRange(transactions);

        // Save to generate transaction IDs.
        context.SaveChanges();

        // 2. Create TopUpRequests.
        var topUpRequests = new[]
        {
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
        };

        context.Set<TopUpRequests>().AddRange(topUpRequests);

        // 3. Create WalletMutations.
        var mutations = new[]
        {
            // Successful top up: incoming mutation.
            new WalletMutations(
                topUpSuccess.Id,
                senderWallet.Id,
                500000m,
                0m,
                500000m,
                WalletMutationDirection.IN
            ),

            // Successful transfer: outgoing mutation.
            new WalletMutations(
                transferSuccess.Id,
                senderWallet.Id,
                75000m,
                500000m,
                425000m,
                WalletMutationDirection.OUT
            ),

            // Successful transfer: incoming mutation.
            new WalletMutations(
                transferSuccess.Id,
                receiverWallet.Id,
                75000m,
                0m,
                75000m,
                WalletMutationDirection.IN
            )
        };

        context.Set<WalletMutations>().AddRange(mutations);

        // 4. Update wallet balances through EF Core.
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

        // 5. Save remaining changes.
        context.SaveChanges();
    }

    public static async Task SeedAsync(
        DbContext context,
        CancellationToken cancellationToken)
    {
        bool alreadySeeded = await context.Set<Transactions>()
            .AnyAsync(
                t => t.ReferenceNo.StartsWith("SEED-"),
                cancellationToken);

        if (alreadySeeded) return;

        var wallets = await context.Set<Wallets>()
            .Where(w => w.Status == WalletStatus.ACTIVE)
            .OrderBy(w => w.Id)
            .Take(2)
            .ToListAsync(cancellationToken);

        if (wallets.Count < 2)
            throw new InvalidOperationException(
                "At least two active wallets are required to begin transactions.");

        var senderWallet = wallets[0];
        var receiverWallet = wallets[1];

        var bankTransfer = await context.Set<PaymentMethods>()
            .FirstOrDefaultAsync(
                pm => pm.Code == PaymentMethodCode.BANK_TRANSFER && pm.IsActive,
                cancellationToken);

        var qris = await context.Set<PaymentMethods>()
            .FirstOrDefaultAsync(
                pm => pm.Code == PaymentMethodCode.QRIS && pm.IsActive,
                cancellationToken);

        if (bankTransfer is null || qris is null)
            throw new InvalidOperationException(
                "Required active payment methods were not found.");

        var now = DateTime.UtcNow.AddHours(7);

        // 1. Create transactions.
        var topUpSuccess = new Transactions(
            "SEED-TOPUP-001",
            senderWallet.Id,
            500000m,
            TransactionType.TOPUP,
            status: TransactionStatus.SUCCESS,
            description: "Dummy successful top up",
            completedAtUtc: now
        );

        var topUpPending = new Transactions(
            "SEED-TOPUP-002",
            senderWallet.Id,
            100000m,
            TransactionType.TOPUP,
            status: TransactionStatus.PENDING,
            description: "Dummy pending top up"
        );

        var topUpFailed = new Transactions(
            "SEED-TOPUP-003",
            senderWallet.Id,
            50000m,
            TransactionType.TOPUP,
            status: TransactionStatus.FAILED,
            description: "Dummy failed top up",
            completedAtUtc: now
        );

        var transferSuccess = new Transactions(
            "SEED-TRANSFER-001",
            receiverWallet.Id,
            75000m,
            TransactionType.TRANSFER,
            senderWalletId: senderWallet.Id,
            status: TransactionStatus.SUCCESS,
            description: "Dummy successful transfer",
            completedAtUtc: now
        );

        var transferFailed = new Transactions(
            "SEED-TRANSFER-002",
            receiverWallet.Id,
            25000m,
            TransactionType.TRANSFER,
            senderWalletId: senderWallet.Id,
            status: TransactionStatus.FAILED,
            description: "Dummy failed transfer",
            completedAtUtc: now
        );

        var transactions = new[]
        {
            topUpSuccess,
            topUpPending,
            topUpFailed,
            transferSuccess,
            transferFailed
        };

        context.Set<Transactions>().AddRange(transactions);

        await context.SaveChangesAsync(cancellationToken);

        // 2. Create TopUpRequests.
        var topUpRequests = new[]
        {
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
        };

        await context.Set<TopUpRequests>()
            .AddRangeAsync(topUpRequests, cancellationToken);

        // 3. Create WalletMutations.
        var mutations = new[]
        {
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
        };

        await context.Set<WalletMutations>()
            .AddRangeAsync(mutations, cancellationToken);

        // 4. Update wallet balances through EF Core.
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

        // 5. Save remaining changes.
        await context.SaveChangesAsync(cancellationToken);
    }
}
