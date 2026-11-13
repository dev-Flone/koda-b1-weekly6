using EWallet.Core.Data;
using EWallet.Core.Interfaces.Data;
using EWallet.Core.Interfaces.Repositories;
using EWallet.Core.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EWallet.Core.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    private readonly AppDbContext _context = context;

    public Task<int> CreateAsync(Users user)
    {
        throw new NotImplementedException();
    }

    public Task<int> CreateAsync(Users user, SqlConnection connection, SqlTransaction transaction)
    {
        throw new NotImplementedException();
    }

    public Task<bool> EmailExistsAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<bool> EmailExistsAsync(string email, int excludeUserId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Users>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Users?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(Users user)
    {
        throw new NotImplementedException();
    }
}