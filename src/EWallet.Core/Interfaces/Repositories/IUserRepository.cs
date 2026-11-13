using EWallet.Core.Models;
using Microsoft.Data.SqlClient;

namespace EWallet.Core.Interfaces.Repositories;

public interface IUserRepository
{
    Task<List<Users>> GetAllAsync();
    Task<Users?> GetByIdAsync(int id);
    Task<int> CreateAsync(Users user);
    Task<int> CreateAsync(Users user, SqlConnection connection, SqlTransaction transaction);
    Task<bool> UpdateAsync(Users user);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> EmailExistsAsync(string email, int excludeUserId);
}