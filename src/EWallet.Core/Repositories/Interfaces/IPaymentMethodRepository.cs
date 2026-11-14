using EWallet.Core.Enums;
using EWallet.Core.Models;

namespace EWallet.Core.Repositories.Interfaces;

public interface IPaymentMethodRepository
{
    Task<List<PaymentMethods>> GetAllAsync(CancellationToken cancellationToken);
    Task<List<PaymentMethods>> GetActive(CancellationToken cancellationToken);
    Task<PaymentMethods?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<PaymentMethods?> GetByCode(PaymentMethodCode code, CancellationToken cancellationToken);

    Task<bool> ExistsById(int id, CancellationToken cancellationToken);
}