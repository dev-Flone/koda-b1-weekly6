namespace EWallet.Core.Dtos.Wallets;

public sealed record WalletResponseDto
{
    public long Id { get; private set; }
    public long UserId { get; private set; }
    public string WalletNumber { get; private set; } = string.Empty;
    public decimal Balance { get; private set; }
    public string Status { get; private set; } = string.Empty;
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
}