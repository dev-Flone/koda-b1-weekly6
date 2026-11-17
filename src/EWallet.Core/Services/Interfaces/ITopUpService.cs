using EWallet.Core.Dtos.TopUps;

namespace EWallet.Core.Services.Interfaces;

public interface ITopUpService
{
    Task<List<TopUpResponseDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<TopUpResponseDto?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<List<TopUpResponseDto>> GetByWalletIdAsync(long walletId, CancellationToken cancellationToken);
    Task<TopUpResponseDto> CreateAsync(CreateTopUpRequest request, CancellationToken cancellationToken);
}