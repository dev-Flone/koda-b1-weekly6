using EWallet.Core.Dtos.Common;
using EWallet.Core.Dtos.Transactions;
using EWallet.Core.Enums;

namespace EWallet.Core.Services.Interfaces;

public interface ITransactionService
{
    Task<List<TransactionResponseDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<TransactionResponseDto?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<TransactionResponseDto?> GetByReferenceNoAsync(string referenceNo, CancellationToken cancellationToken);
    Task<List<TransactionResponseDto>> GetByWalletIdAsync(long walletId, CancellationToken cancellationToken);
    Task<List<TransactionResponseDto>> GetByStatusAsync(TransactionStatus status, CancellationToken cancellationToken);
    Task<List<TransactionResponseDto>> GetByTypeAsync(TransactionType type, CancellationToken cancellationToken);
    Task<PaginationResponse<TransactionResponseDto>> GetHistoryAsync(TransactionQueryDto query, CancellationToken cancellationToken);
}
