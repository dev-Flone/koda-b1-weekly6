using System.ComponentModel.DataAnnotations;
using EWallet.Core.Enums;

namespace EWallet.Core.Models;

public sealed class AuditLogs
{
    public long Id { get; private set; }

    // FK UserId
    public long? ActorUserId { get; private set; }

    // FK TransactionId
    public long? TransactionId { get; private set; }

    [Required] public AuditLogAction Action { get; private set; }
    [Required] public AuditLogEntityType Type { get; private set; }
    [StringLength(60)] public string? EntityId { get; private set; }
    [Required] public AuditLogOutcome Outcome { get; private set; }
    [StringLength(500)] public string? Description { get; private set; }
    [StringLength(100)] public string? CorrelationId { get; private set; }
    [Required] public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow.AddHours(7);

    // Relationship
    public Users? User { get; private set; } = null!;
    public Transactions? Transaction { get; private set; } = null!;
}