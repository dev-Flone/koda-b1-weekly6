namespace EWallet.Core.Dtos.AuditLogs;

public sealed record AuditLogResponseDto
{
    public long Id { get; private set; }
    public long? ActorUserId { get; private set; }
    public long? TransactionId { get; private set; }
    public string Action { get; private set; } = string.Empty;
    public string Type { get; private set; } = string.Empty;
    public string? EntityId { get; private set; }
    public string Outcome { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? CorrelationId { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
}