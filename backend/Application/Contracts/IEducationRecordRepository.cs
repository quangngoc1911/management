using ManagementSystem.Modules.Education.Application.DTOs;
using ManagementSystem.Modules.Education.Domain.Entities;

namespace ManagementSystem.Application.Contracts;

public interface IEducationRecordRepository
{
    Task<(IReadOnlyList<EducationRecord> Items, int TotalCount)> GetPagedAsync(EducationRecordQueryParams query);
    Task<EducationRecord?> GetByIdAsync(Guid id);
    Task<EducationRecord?> GetForUpdateAsync(Guid id);
    Task CreateAsync(EducationRecord record);
    Task<bool> DeleteAsync(Guid id);
}
