using EWallet.Core.Dtos.PaymentMethods;
using EWallet.Core.Enums;

namespace EWallet.Core.Services.Interfaces;

public interface IPaymentMethodService
{
    Task<List<PaymentMethodResponseDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<List<PaymentMethodResponseDto>> GetActiveAsync(CancellationToken cancellationToken);
    Task<PaymentMethodResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<PaymentMethodResponseDto?> GetByCodeAsync(PaymentMethodCode code, CancellationToken cancellationToken);
    Task<bool> ExistsByIdAsync(int id, CancellationToken cancellationToken);
}