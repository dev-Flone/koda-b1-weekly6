using EWallet.Core.Enums;
using EWallet.Core.Models;

namespace EWallet.Core.Repositories.Interfaces;

public interface IPaymentMethodRepository
{
    Task<List<PaymentMethods>> GetAllAsync(CancellationToken cancellationToken);
    Task<List<PaymentMethods>> GetActiveAsync(CancellationToken cancellationToken);
    Task<PaymentMethods?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<PaymentMethods?> GetByCodeAsync(PaymentMethodCode code, CancellationToken cancellationToken);

    Task<bool> ExistsByIdAsync(int id, CancellationToken cancellationToken);
}