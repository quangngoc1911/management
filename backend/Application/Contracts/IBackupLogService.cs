using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.SystemAdmin.Application.DTOs;

namespace ManagementSystem.Application.Contracts;

/// <summary>Read-only access to backup history.</summary>
public interface IBackupLogService
{
    Task<PaginatedResultDto<BackupLogDto>> GetPagedAsync(BackupLogQueryParams query);
    Task<BackupLogDto?> GetByIdAsync(Guid id);
}
