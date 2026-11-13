using EWallet.Core.Data;
using EWallet.Core.Enums;
using EWallet.Core.Interfaces.Data;
using EWallet.Core.Interfaces.Repositories;
using EWallet.Core.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EWallet.Core.Repositories;

public class TransactionRepository(AppDbContext context) : ITransactionRepository
{
    private readonly AppDbContext _context = context;

    public Task<int> CreateAsync(Transactions transaction, SqlConnection connection, SqlTransaction dbTransaction)
    {
        throw new NotImplementedException();
    }

    public Task<int> CreateAsync(Transactions transaction)
    {
        throw new NotImplementedException();
    }

    public Task<List<Transactions>> GetByUserIdAsync(int userId)
    {
        throw new NotImplementedException();
    }
}