

using EWallet.Core.Data;
using EWallet.Core.Dtos.Wallets;
using EWallet.Core.Enums;
using EWallet.Core.Models;
using EWallet.Core.Repositories.Interfaces;
using EWallet.Core.Services.Interfaces;

namespace EWallet.Core.Services;

public class WalletService(AppDbContext context, IWalletRepository repository) : IWalletService
{
    public async Task<List<WalletResponseDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var wallets = await repository.GetAllAsync(cancellationToken);

        return [.. wallets.Select(MapToResponse)];
    }

    public async Task<WalletResponseDto?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var wallet = await repository.GetByIdAsync(id, cancellationToken);

        return wallet is null ? null : MapToResponse(wallet);
    }

    public async Task<List<WalletResponseDto>> GetByStatus(WalletStatus status, CancellationToken cancellationToken)
    {
        var wallets = await repository.GetByStatus(status, cancellationToken);

        return [.. wallets.Select(MapToResponse)];
    }

    public async Task<WalletResponseDto?> GetByUserIdAsync(long userId, CancellationToken cancellationToken)
    {
        var wallet = await repository.GetByUserId(userId, cancellationToken);

        return wallet is null ? null : MapToResponse(wallet);
    }

    public async Task<WalletResponseDto?> GetByWalletNumberAsync(string walletNumber, CancellationToken cancellationToken)
    {
        var wallet = await repository.GetByWalletNumberAsync(walletNumber, cancellationToken);

        return wallet is null ? null : MapToResponse(wallet);
    }

    public async Task<WalletResponseDto?> UpdateStatusAsync(long id, WalletStatusUpdateDto walletStatusUpdate, CancellationToken cancellationToken)
    {
        var wallet = await repository.GetByIdAsync(id, cancellationToken);

        if (wallet is null) return null;

        wallet.UpdateStatus(walletStatusUpdate.Status);

        repository.Update(wallet);

        await context.SaveChangesAsync(cancellationToken);

        return MapToResponse(wallet);
    }

    // Methods
    private static WalletResponseDto MapToResponse(Wallets wallet)
    {
        return new WalletResponseDto
        {
            Id = wallet.Id,
            UserId = wallet.UserId,
            WalletNumber = wallet.WalletNumber,
            Balance = wallet.Balance,
            Status = wallet.Status.ToString(),
            CreatedAtUtc = wallet.CreatedAtUtc,
            UpdatedAtUtc = wallet.UpdatedAtUtc
        };
    }
}