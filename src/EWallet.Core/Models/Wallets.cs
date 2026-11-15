using System.ComponentModel.DataAnnotations;
using EWallet.Core.Enums;

namespace EWallet.Core.Models;

public class Wallets(long userId, string walletNumber, decimal balance)
{
    // PK
    public long Id { get; private set; }

    // FK
    public long UserId { get; private set; } = userId;

    [Required]
    [MaxLength(30)]
    public string WalletNumber { get; private set; } = walletNumber;

    [Required]
    public decimal Balance { get; private set; } = balance;

    [Required]
    [StringLength(30)]
    public WalletStatus Status { get; private set; } = WalletStatus.ACTIVE;

    [Required]
    public byte[] RowVersion { get; private set; } = null!;

    [Required]
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow.AddHours(7);

    public DateTime? UpdatedAtUtc { get; private set; }

    // FK Reference
    public Users Users { get; private set; } = null!;

    // Relationship
    public ICollection<Transactions> SentTransactions { get; private set; } = [];
    public ICollection<Transactions> ReceivedTransactions { get; private set; } = [];
    public ICollection<WalletMutations> WalletMutations { get; private set; } = [];
    public ICollection<TopUpRequests> TopUpRequests { get; private set; } = [];

    // Methods
    public void UpdateStatus(WalletStatus status)
    {
        Status = status;
        UpdatedAtUtc = DateTime.UtcNow.AddHours(7);
    }
}