using EWallet.Core.Enums;
using EWallet.Core.Models;
using EWallet.Core.Dtos.Common;
using EWallet.Core.Dtos.Transfer;
using EWallet.Core.Dtos.Transactions;

namespace EWallet.Core.Repositories.Interfaces;

public interface ITransactionRepository
{
    Task<List<Transactions>> GetAllAsync(CancellationToken cancellationToken);
    Task<Transactions?> GetByReferenceNoAsync(string referenceNo, CancellationToken cancellationToken);
    Task<Transactions?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<List<Transactions>> GetByWalletId(long walletId, CancellationToken cancellationToken);
    Task<List<Transactions>> GetByTypeAsync(TransactionType type, CancellationToken cancellationToken);
    Task<List<Transactions>> GetByStatusAsync(TransactionStatus status, CancellationToken cancellationToken);

    Task<bool> ExistsByReferenceNoAsync(string referenceNo, CancellationToken cancellationToken);

    Task AddAsync(Transactions transaction, CancellationToken cancellationToken);
    void Update(Transactions transaction);
    Task<PaginationResponse<TransactionResponseDto>> GetHistoryAsync(TransactionQueryDto query, CancellationToken cancellationToken);
}