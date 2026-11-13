using EWallet.Core.Models;

namespace EWallet.Core.Interfaces.Services;

public interface IUserService
{
    Task<List<Users>> GetAllAsync();
    Task<Users?> GetByIdAsync(int id);
    Task<int> CreateAsync(Users user);
    Task<bool> UpdateAsync(Users user);
}