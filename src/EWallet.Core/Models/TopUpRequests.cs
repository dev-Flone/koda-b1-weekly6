using System.ComponentModel.DataAnnotations;
using EWallet.Core.Enums;

namespace EWallet.Core.Models;

public sealed class TopUpRequests(long transactionId, long walletId, int paymentMethodId, decimal amount)
{
    public long Id { get; private set; }

    // FK TransactionId
    public long TransactionId { get; private set; } = transactionId;

    // FK WalletId
    public long WalletId { get; private set; } = walletId;

    // FK PaymentMethodId
    public int PaymentMethodId { get; private set; } = paymentMethodId;

    [Required]
    public decimal Amount { get; private set; } = amount;

    [Required]
    [StringLength(20)]
    public TopUpRequestStatus Status { get; private set; } = TopUpRequestStatus.PENDING;

    [StringLength(100)]
    public string? ProviderReference { get; private set; }

    [Required]
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
    public DateTime? ExpiresAtUtc { get; private set; }
    public DateTime? PaidAtUtc { get; private set; }

    // Relationship
    public Transactions Transaction { get; private set; } = null!;
    public Wallets Wallet { get; private set; } = null!;
    public PaymentMethods PaymentMethod { get; private set; } = null!;
}