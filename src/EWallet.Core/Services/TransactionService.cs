using EWallet.Core.Data;
using EWallet.Core.Dtos.Common;
using EWallet.Core.Dtos.Transactions;
using EWallet.Core.Enums;
using EWallet.Core.Models;
using EWallet.Core.Repositories.Interfaces;
using EWallet.Core.Services.Interfaces;

namespace EWallet.Core.Services;

public class TransactionService(ITransactionRepository repository) : ITransactionService
{
    public async Task<List<TransactionResponseDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var transaction = await repository.GetAllAsync(cancellationToken);

        return [.. transaction.Select(MapToResponse)];
    }

    public async Task<TransactionResponseDto?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var transaction = await repository.GetByIdAsync(id, cancellationToken);

        return transaction is null ? null : MapToResponse(transaction);
    }

    public async Task<TransactionResponseDto?> GetByReferenceNoAsync(string referenceNo, CancellationToken cancellationToken)
    {
        var transaction = await repository.GetByReferenceNoAsync(referenceNo, cancellationToken);

        return transaction is null ? null : MapToResponse(transaction);
    }

    public async Task<List<TransactionResponseDto>> GetByStatusAsync(TransactionStatus status, CancellationToken cancellationToken)
    {
        var transaction = await repository.GetByStatusAsync(status, cancellationToken);

        return [.. transaction.Select(MapToResponse)];
    }

    public async Task<List<TransactionResponseDto>> GetByTypeAsync(TransactionType type, CancellationToken cancellationToken)
    {
        var transaction = await repository.GetByTypeAsync(type, cancellationToken);

        return [.. transaction.Select(MapToResponse)];
    }

    public async Task<List<TransactionResponseDto>> GetByWalletIdAsync(long walletId, CancellationToken cancellationToken)
    {
        var transaction = await repository.GetByWalletId(walletId, cancellationToken);

        return [.. transaction.Select(MapToResponse)];
    }

    public async Task<PaginationResponse<TransactionResponseDto>> GetHistoryAsync(TransactionQueryDto query, CancellationToken cancellationToken)
    {
        return await repository.GetHistoryAsync(query, cancellationToken);
    }

    // Methods
    private static TransactionResponseDto MapToResponse(
        Transactions transaction)
    {
        return new TransactionResponseDto
        {
            Id = transaction.Id,
            ReferenceNo = transaction.ReferenceNo,
            SenderWalletId = transaction.SenderWalletId,
            ReceiverWalletId = transaction.ReceiverWalletId,
            Type = transaction.Type.ToString(),
            Amount = transaction.Amount,
            Status = transaction.Status.ToString(),
            Description = transaction.Description,
            CreatedAtUtc = transaction.CreatedAtUtc,
            CompletedAtUtc = transaction.CompletedAtUtc
        };
    }
}