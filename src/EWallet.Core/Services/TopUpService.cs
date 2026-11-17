using EWallet.Core.Data;
using EWallet.Core.Dtos.TopUps;
using EWallet.Core.Enums;
using EWallet.Core.Repositories.Interfaces;
using EWallet.Core.Services.Interfaces;
using EWallet.Core.Models;

namespace EWallet.Core.Services;

public sealed class TopUpService(
    AppDbContext context,
    ITopUpRequestRepository topUpRequestRepository,
    ITransactionRepository transactionRepository,
    IWalletRepository walletRepository,
    IPaymentMethodRepository paymentMethodRepository
) : ITopUpService
{
    public async Task<TopUpResponseDto> CreateAsync(CreateTopUpRequest request, CancellationToken cancellationToken)
    {
        var wallet = await walletRepository.GetByIdAsync(request.WalletId, cancellationToken) ?? throw new KeyNotFoundException("Wallet was not found.");

        if (wallet.Status != WalletStatus.ACTIVE) throw new InvalidOperationException("Wallet is not active");

        var paymentMethod = await paymentMethodRepository.GetByIdAsync(request.PaymentMethodId, cancellationToken) ?? throw new KeyNotFoundException("Payment method was not found");

        if (!paymentMethod.IsActive) throw new InvalidOperationException("Payment method is not active");

        await using var dbTransaction = await context.Database.BeginTransactionAsync(cancellationToken);

        var transaction = new Transactions(
            referenceNo: GenerateReferenceNo(),
            receiverWalletId: wallet.Id,
            amount: request.Amount,
            type: TransactionType.TOPUP,
            status: TransactionStatus.PENDING,
            description: $"Top Up via {paymentMethod.Name}"
        );

        await transactionRepository.AddAsync(transaction, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        var topUpRequest = new TopUpRequests(
            transactionId: transaction.Id,
            walletId: wallet.Id,
            paymentMethodId: paymentMethod.Id,
            amount: request.Amount,
            status: TopUpRequestStatus.PENDING
        );

        await topUpRequestRepository.AddAsync(topUpRequest, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        await dbTransaction.CommitAsync(cancellationToken);

        return MapToResponse(topUpRequest);
    }

    public async Task<List<TopUpResponseDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var request = await topUpRequestRepository.GetAllAsync(cancellationToken);

        return [.. request.Select(MapToResponse)];
    }

    public async Task<List<TopUpResponseDto>> GetByWalletIdAsync(long id, CancellationToken cancellationToken)
    {
        var requests = await topUpRequestRepository.GetByWalletIdAsync(id, cancellationToken);

        return [.. requests.Select(MapToResponse)];
    }


    // Methods
    private static string GenerateReferenceNo()
    {
        return $"TOP{DateTime.UtcNow:yyyyMMddHHmmssfff}" + Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();
    }

    private static TopUpResponseDto MapToResponse(TopUpRequests request)
    {
        return new TopUpResponseDto
        {
            Id = request.Id,
            TransactionId = request.TransactionId,
            WalletId = request.WalletId,
            PaymentMethodId = request.PaymentMethodId,
            Amount = request.Amount,
            Status = request.Status.ToString(),
            ProviderReference = request.ProviderReference,
            CreatedAtUtc = request.CreatedAtUtc,
            ExpiresAtUtc = request.ExpiresAtUtc,
            PaidAtUtc = request.PaidAtUtc
        };
    }

    public async Task<TopUpResponseDto?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        var request = await topUpRequestRepository.GetByIdAsync(id, cancellationToken);

        return request is null ? null : MapToResponse(request);
    }
}