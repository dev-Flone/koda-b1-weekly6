namespace EWallet.Core.Dtos.Transactions;

public sealed record TransactionResponseDto
{
    public long Id { get; private set; }
    public string ReferenceNo { get; private set; } = string.Empty;
    public long? SenderWalletId { get; private set; }
    public long ReceiverWalletId { get; private set; }
    public string Type { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public string Status { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }
}