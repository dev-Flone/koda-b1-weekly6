using EWallet.Core.Interfaces.Data;
using EWallet.Core.Interfaces.Repositories;
using EWallet.Core.Interfaces.Services;
using EWallet.Core.Models;
using EWallet.Core.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using EWallet.Core.Data;

namespace EWallet.Core.Services;

public class WalletService(IWalletRepository walletRepository, ITransactionRepository transactionRepository, AppDbContext context) : IWalletService
{
    private readonly IWalletRepository _walletRepository = walletRepository;
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    private readonly AppDbContext _context = context;

    public Task<Wallets?> GetByUserIdAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> TopUpAsync(int userId, decimal amount, string? description)
    {
        throw new NotImplementedException();
    }
}