using EWallet.Core.Data;
using EWallet.Core.Interfaces.Data;
using EWallet.Core.Repositories.Interfaces;
using EWallet.Core.Interfaces.Services;
using EWallet.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EWallet.Core.Services;

public class UserService(IUserRepository userRepository, IWalletRepository walletRepository, AppDbContext context) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IWalletRepository _walletRepository = walletRepository;
    private readonly AppDbContext _context = context;

    public Task<int> CreateAsync(Users user)
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