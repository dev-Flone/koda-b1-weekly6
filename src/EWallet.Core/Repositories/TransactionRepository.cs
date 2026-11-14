using EWallet.Core.Data;
using EWallet.Core.Enums;
using EWallet.Core.Models;
using EWallet.Core.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EWallet.Core.Repositories;

public class TransactionRepository(AppDbContext context) : ITransactionRepository
{
    public async Task<List<Transactions>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Transactions
                .AsNoTracking()
                .OrderByDescending(t => t.CreatedAtUtc)
                .ToListAsync(cancellationToken);
    }

    public async Task<Transactions?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await context.Transactions.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<Transactions?> GetByReferenceNoAsync(string referenceNo, CancellationToken cancellationToken)
    {
        return await context.Transactions.FirstOrDefaultAsync(t => t.ReferenceNo == referenceNo, cancellationToken);
    }

    public async Task<List<Transactions>> GetByWalletId(long walletId, CancellationToken cancellationToken)
    {
        return await context.Transactions
                .AsNoTracking()
                .Where(t => t.SenderWalletId == walletId || t.ReceiverWalletId == walletId)
                .OrderByDescending(t => t.CreatedAtUtc)
                .ToListAsync(cancellationToken);
    }

    public async Task<List<Transactions>> GetByStatusAsync(TransactionStatus status, CancellationToken cancellationToken)
    {
        return await context.Transactions
                .AsNoTracking()
                .Where(t => t.Status == status)
                .OrderByDescending(t => t.CreatedAtUtc)
                .ToListAsync(cancellationToken);
    }

    public async Task<List<Transactions>> GetByTypeAsync(TransactionType type, CancellationToken cancellationToken)
    {
        return await context.Transactions.AsNoTracking().Where(t => t.Type == type).OrderByDescending(t => t.CreatedAtUtc).ToListAsync(cancellationToken);
    }


    // EXISTS
    public async Task<bool> ExistsByReferenceNoAsync(string referenceNo, CancellationToken cancellationToken)
    {
        return await context.Transactions.AnyAsync(t => t.ReferenceNo == referenceNo, cancellationToken);
    }


    // METHODS
    public async Task AddAsync(Transactions transaction, CancellationToken cancellationToken)
    {
        await context.Transactions.AddAsync(transaction, cancellationToken);
    }

    public void Update(Transactions transaction)
    {
        context.Transactions.Update(transaction);
    }

}