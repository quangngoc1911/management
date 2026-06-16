using ManagementSystem.Modules.Education.Application.DTOs;
using ManagementSystem.Modules.Education.Domain.Entities;

namespace ManagementSystem.Application.Contracts;

public interface IStudyScheduleRepository
{
    Task<(IReadOnlyList<StudySchedule> Items, int TotalCount)> GetPagedAsync(StudyScheduleQueryParams query);
    Task<StudySchedule?> GetByIdAsync(Guid id);
    Task<StudySchedule?> GetForUpdateAsync(Guid id);
    Task CreateAsync(StudySchedule schedule);
    Task<bool> DeleteAsync(Guid id);
}
