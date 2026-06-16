using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.SystemAdmin.Application.DTOs;

namespace ManagementSystem.Application.Contracts;

public interface IBackupLogRepository
{
    Task<(IReadOnlyList<BackupLog> Items, int TotalCount)> GetPagedAsync(BackupLogQueryParams query);
    Task<BackupLog?> GetByIdAsync(Guid id);
}
