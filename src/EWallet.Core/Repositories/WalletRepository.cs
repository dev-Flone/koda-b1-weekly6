using EWallet.Core.Data;
using EWallet.Core.Interfaces.Data;
using EWallet.Core.Interfaces.Repositories;
using EWallet.Core.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EWallet.Core.Repositories;

public class WalletRepository(AppDbContext context) : IWalletRepository
{
    private readonly AppDbContext _context = context;

    public Task<bool> AddBalanceAsync(int walletId, decimal amount, SqlConnection connection, SqlTransaction transaction)
    {
        throw new NotImplementedException();
    }

    public Task<int> CreateAsync(Wallets wallet, SqlConnection connection, SqlTransaction transaction)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DebitBalanceAsync(int walletId, decimal amount, SqlConnection connection, SqlTransaction transaction)
    {
        throw new NotImplementedException();
    }

    public Task<Wallets?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Wallets?> GetByUserIdAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateBalanceAsync(int walletId, decimal balance)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateBalanceAsync(int walletId, decimal balance, SqlConnection connection, SqlTransaction transaction)
    {
        throw new NotImplementedException();
    }
}