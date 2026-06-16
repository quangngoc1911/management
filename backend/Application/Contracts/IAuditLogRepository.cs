using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.SystemAdmin.Application.DTOs;

namespace ManagementSystem.Application.Contracts;

public interface IAuditLogRepository
{
    Task<(IReadOnlyList<AuditLog> Items, int TotalCount)> GetPagedAsync(AuditLogQueryParams query);
    Task<AuditLog?> GetByIdAsync(Guid id);
}
