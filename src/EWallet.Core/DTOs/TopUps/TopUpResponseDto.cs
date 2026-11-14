namespace EWallet.Core.Dtos.TopUp;

public sealed record TopUpResponseDto
{
    public long Id { get; private set; }
    public long TransactionId { get; private set; }
    public long WalletId { get; private set; }
    public int PaymentMethodId { get; private set; }
    public decimal Amount { get; private set; }
    public string Status { get; private set; } = string.Empty;
    public string? ProviderReference { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? ExpiresAtUtc { get; private set; }
    public DateTime? PaidAtUtc { get; private set; }
}