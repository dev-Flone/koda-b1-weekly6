using System.ComponentModel.DataAnnotations;

namespace EWallet.Core.Dtos.TopUp;

public sealed record CreateTopUpRequest
{
    [Required] public long WalletId { get; private set; }
    [Required] public int PaymentMethod { get; private set; }
    [Range(typeof(decimal), "0.01", "9999999999999999")] public decimal Amount { get; private set; }

}