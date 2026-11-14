using EWallet.Core.Data;
using EWallet.Core.Enums;
using EWallet.Core.Models;
using EWallet.Core.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EWallet.Core.Repositories;

public sealed class PaymentMethodRepository(AppDbContext context) : IPaymentMethodRepository
{
    public async Task<List<PaymentMethods>> GetActive(CancellationToken cancellationToken)
    {
        return await context.PaymentMethods.AsNoTracking().Where(pm => pm.IsActive).ToListAsync(cancellationToken);
    }

    public async Task<List<PaymentMethods>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.PaymentMethods.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<PaymentMethods?> GetByCode(PaymentMethodCode code, CancellationToken cancellationToken)
    {
        return await context.PaymentMethods.FirstOrDefaultAsync(pm => pm.Code == code, cancellationToken);
    }

    public async Task<PaymentMethods?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await context.PaymentMethods.FirstOrDefaultAsync(pm => pm.Id == id, cancellationToken);
    }


    // EXISTS
    public async Task<bool> ExistsById(int id, CancellationToken cancellationToken)
    {
        return await context.PaymentMethods.AnyAsync(pm => pm.Id == id, cancellationToken);
    }
}