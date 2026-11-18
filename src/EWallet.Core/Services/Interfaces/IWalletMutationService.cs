using EWallet.Core.Dtos.WalletMutations;
using EWallet.Core.Enums;

namespace EWallet.Core.Services.Interfaces;

public interface IWalletMutationService
{
    Task<List<WalletMutationResponseDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<WalletMutationResponseDto?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<List<WalletMutationResponseDto>> GetByWalletIdAsync(long walletId, CancellationToken cancellationToken);
    Task<List<WalletMutationResponseDto>> GetByTransactionIdAsync(long transactionId, CancellationToken cancellationToken);
    Task<List<WalletMutationResponseDto>> GetByDirectionAsync(WalletMutationDirection direction, CancellationToken cancellationToken);
}