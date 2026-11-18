using EWallet.Core.Enums;

namespace EWallet.Core.Dtos.WalletMutations;

public sealed record WalletMutationResponseDto(
    long Id,
    long TransactionId,
    long WalletId,
    WalletMutationDirection Direction,
    decimal Amount,
    decimal BalanceBefore,
    decimal BalanceAfter,
    DateTime CreatedAtUtc
);