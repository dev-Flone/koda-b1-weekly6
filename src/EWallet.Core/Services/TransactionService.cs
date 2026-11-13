using EWallet.Core.Enums;
using EWallet.Core.Interfaces.Data;
using EWallet.Core.Interfaces.Repositories;
using EWallet.Core.Interfaces.Services;
using EWallet.Core.Models;
using Microsoft.Data.SqlClient;
using EWallet.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using EWallet.Core.Data;

namespace EWallet.Core.Services;

public class TransactionService(ITransactionRepository transactionRepository, IWalletRepository walletRepository, AppDbContext context) : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    private readonly IWalletRepository _walletRepository = walletRepository;
    private readonly AppDbContext _context = context;

    public Task<List<Transactions>> GetByUserId(int userId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> TransferAsync(int senderUserId, int receiverUserId, decimal amount, string? description)
    {
        throw new NotImplementedException();
    }
}