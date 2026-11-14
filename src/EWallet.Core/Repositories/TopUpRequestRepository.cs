using EWallet.Core.Data;
using EWallet.Core.Enums;
using EWallet.Core.Models;
using EWallet.Core.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EWallet.Core.Repositories;

public sealed class TopUpRequestRepository(AppDbContext context) : ITopUpRequestRepository
{
    public async Task<List<TopUpRequests>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.TopUpRequests
                .AsNoTracking()
                .OrderByDescending(tr => tr.CreatedAtUtc)
                .ToListAsync(cancellationToken);
    }

    public async Task<TopUpRequests?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await context.TopUpRequests.FirstOrDefaultAsync(tr => tr.Id == id, cancellationToken);
    }

    public async Task<TopUpRequests?> GetByProviderReferenceAsync(string providerReference, CancellationToken cancellationToken)
    {
        return await context.TopUpRequests.FirstOrDefaultAsync(tr => tr.ProviderReference == providerReference, cancellationToken);
    }

    public async Task<List<TopUpRequests>> GetByStatus(TopUpRequestStatus status, CancellationToken cancellationToken)
    {
        return await context.TopUpRequests
                .AsNoTracking()
                .Where(tr => tr.Status == status)
                .OrderByDescending(tr => tr.CreatedAtUtc)
                .ToListAsync(cancellationToken);
    }

    public async Task<TopUpRequests?> GetByTransactionIdAsync(long transactionId, CancellationToken cancellationToken)
    {
        return await context.TopUpRequests.FirstOrDefaultAsync(tr => tr.TransactionId == transactionId, cancellationToken);
    }

    public async Task<List<TopUpRequests>> GetByWalletIdAsync(long walletId, CancellationToken cancellationToken)
    {
        return await context.TopUpRequests
                .AsNoTracking()
                .Where(tr => tr.WalletId == walletId)
                .OrderByDescending(tr => tr.CreatedAtUtc)
                .ToListAsync(cancellationToken);
    }


    // METHODS
    public async Task AddAsync(TopUpRequests request, CancellationToken cancellationToken)
    {
        await context.TopUpRequests.AddAsync(request, cancellationToken);
    }

    public void Update(TopUpRequests request)
    {
        context.TopUpRequests.Update(request);
    }
}