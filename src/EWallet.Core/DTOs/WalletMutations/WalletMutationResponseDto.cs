namespace EWallet.Core.Dtos.WalletMutations;

public sealed record WalletMutationResponseDto
{
    public long Id { get; private set; }
    public long TransactionId { get; private set; }
    public long WalletId { get; private set; }
    public string Direction { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public decimal BalanceBefore { get; private set; }
    public decimal BalanceAfter { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
}