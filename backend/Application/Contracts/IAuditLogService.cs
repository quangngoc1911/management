using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.SystemAdmin.Application.DTOs;

namespace ManagementSystem.Application.Contracts;

/// <summary>Read-only access to the audit trail.</summary>
public interface IAuditLogService
{
    Task<PaginatedResultDto<AuditLogDto>> GetPagedAsync(AuditLogQueryParams query);
    Task<AuditLogDto?> GetByIdAsync(Guid id);
}
