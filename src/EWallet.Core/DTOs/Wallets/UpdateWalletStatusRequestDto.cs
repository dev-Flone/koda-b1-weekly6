using System.ComponentModel.DataAnnotations;
using EWallet.Core.Enums;

namespace EWallet.Core.Dtos.Wallets;

public sealed record WalletStatusUpdateDto
{
    [Required, EnumDataType(typeof(WalletStatus))]
    public WalletStatus Status { get; private set; }
}