using EWallet.Core.Models;
using Microsoft.Data.SqlClient;

namespace EWallet.Core.Interfaces.Repositories;

public interface ITransactionRepository
{
    Task<int> CreateAsync(Transactions transaction, SqlConnection connection, SqlTransaction dbTransaction);
    Task<int> CreateAsync(Transactions transaction);
    Task<List<Transactions>> GetByUserIdAsync(int userId);
}