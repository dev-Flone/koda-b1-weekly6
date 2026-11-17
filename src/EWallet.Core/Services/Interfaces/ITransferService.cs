using EWallet.Core.Dtos.Transactions;
using EWallet.Core.Dtos.Transfer;

namespace EWallet.Core.Services.Interfaces;

public interface ITransferService
{
    Task<List<TransferResponseDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<TransferResponseDto?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<List<TransferResponseDto>> GetByWalletIdAsync(long walletId, CancellationToken cancellationToken);
    Task<TransferResponseDto> CreateAsync(CreateTransferRequestDto request, CancellationToken cancellationToken);
}