using EWallet.Core.Dtos.AuditLogs;
using EWallet.Core.Enums;
using EWallet.Core.Models;

namespace EWallet.Core.Interfaces;

public interface IAuditLogService
{
    Task<List<AuditLogResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<AuditLogResponseDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<List<AuditLogResponseDto>> GetByActorUserIdAsync(long actorUserId, CancellationToken cancellationToken = default);

    Task<List<AuditLogResponseDto>> GetByTransactionIdAsync(long transactionId, CancellationToken cancellationToken = default);

    Task<List<AuditLogResponseDto>> GetByActionAsync(AuditLogAction action, CancellationToken cancellationToken = default);

    Task<List<AuditLogResponseDto>> GetByEntityTypeAsync(AuditLogEntityType type, CancellationToken cancellationToken = default);

    Task<List<AuditLogResponseDto>> GetByOutcomeAsync(AuditLogOutcome outcome, CancellationToken cancellationToken = default);

    Task AddAsync(AuditLogs auditLog, CancellationToken cancellationToken = default);
}