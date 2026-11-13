using System.ComponentModel.DataAnnotations;
using EWallet.Core.Enums;

namespace EWallet.Core.Models;

public sealed class Transactions(string referenceNo, long receiverWalletId, decimal amount)
{
    public long Id { get; private set; }

    [Required]
    [StringLength(50)]
    public string ReferenceNo { get; private set; } = referenceNo;
    public long? SenderWalletId { get; private set; }

    [Required]
    public long ReceiverWalletId { get; private set; } = receiverWalletId;

    [Required]
    [StringLength(20)]
    public TransactionType Type { get; private set; }

    [Required]
    public decimal Amount { get; private set; } = amount;

    [Required]
    [StringLength(20)]
    public TransactionStatus Status { get; private set; } = TransactionStatus.PENDING;

    [MaxLength(255)]
    public string? Description { get; private set; }

    [Required]
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
    public DateTime? CompletedAtUtc { get; private set; }

    // Relationship
    public Wallets? SenderWallet { get; private set; }
    public Wallets ReceiverWallet { get; private set; } = null!;
    public TopUpRequests? TopUpRequest { get; private set; }
    public ICollection<WalletMutations> WalletMutations { get; private set; } = null!;
    public ICollection<AuditLogs> AuditLogs { get; private set; } = null!;
}