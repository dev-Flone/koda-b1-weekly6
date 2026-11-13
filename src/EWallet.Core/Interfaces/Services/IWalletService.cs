using EWallet.Core.Models;

namespace EWallet.Core.Interfaces.Services;

public interface IWalletService
{
    Task<Wallets?> GetByUserIdAsync(int userId);
    Task<bool> TopUpAsync(int userId, decimal amount, string? description);
}