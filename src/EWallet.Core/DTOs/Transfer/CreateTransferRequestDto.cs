using System.ComponentModel.DataAnnotations;

namespace EWallet.Core.Dtos.Transfer;

public sealed record CreateTransferRequestDto
{
    [Required] public long SenderWalletId { get; private set; }
    [Required] public long ReceiverWalletId { get; private set; }
    [Range(typeof(decimal), "0.01", "999999999999999999999")] public decimal Amount { get; private set; }
    [StringLength(250)] public string? Description { get; private set; }
}