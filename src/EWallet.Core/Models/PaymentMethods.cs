using System.ComponentModel.DataAnnotations;
using EWallet.Core.Enums;

namespace EWallet.Core.Models;

public sealed class PaymentMethods(int id, string name, bool isActive)
{
    public int Id { get; private set; } = id;

    [StringLength(30)]
    public PaymentMethodCode Code { get; private set; }

    [Required]
    [StringLength(100)]
    public string Name { get; private set; } = name;

    [StringLength(100)]
    public string? ProviderName { get; private set; }

    [Required]
    public bool IsActive { get; private set; } = isActive;

    [Required]
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;

    // Relationship
    public ICollection<TopUpRequests> TopUpRequests { get; private set; } = null!;
}