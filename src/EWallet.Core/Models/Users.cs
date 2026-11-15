using System.ComponentModel.DataAnnotations;
using EWallet.Core.Enums;

namespace EWallet.Core.Models;

public sealed class Users(string userName, string fullName, string email, string phone)
{
    public long Id { get; private set; }

    [Required]
    [MaxLength(50)]
    public string UserName { get; private set; } = userName;

    [Required]
    [MaxLength(150)]
    public string FullName { get; private set; } = fullName;

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; private set; } = email;

    [Required]
    [MaxLength(15)]
    public string Phone { get; private set; } = phone;

    [MaxLength(500)]
    public string? PasswordHash { get; private set; }

    [Required]
    public UserStatus Status { get; private set; } = UserStatus.ACTIVE;

    [Required]
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow.AddHours(7);

    public DateTime? UpdatedAtUtc { get; private set; }

    // Relationship
    public Wallets Wallets { get; private set; } = null!;
    public ICollection<AuditLogs> AuditLogs { get; private set; } = [];

    // Methods
    public void Suspend()
    {
        Status = UserStatus.SUSPENDED;
        UpdatedAtUtc = DateTime.UtcNow.AddHours(7);
    }

    public void UpdateProfile(string username, string fullname, string email, string phone)
    {
        UserName = username;
        FullName = fullname;
        Email = email;
        Phone = phone;
    }
}