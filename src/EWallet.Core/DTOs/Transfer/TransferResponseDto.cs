namespace EWallet.Core.Dtos.Transfer;

public sealed record TransferResponseDto
{
    public long TransactionId { get; private set; }
    public string ReferenceNo { get; private set; } = string.Empty;
    public long SenderWalletId { get; private set; }
    public long ReceiverWalletId { get; private set; }
    public decimal Amount { get; private set; }
    public string Status { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? CompletedAtUtc { get; private set; }
}