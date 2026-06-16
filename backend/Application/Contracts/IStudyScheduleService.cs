using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Education.Application.DTOs;

namespace ManagementSystem.Application.Contracts;

public interface IStudyScheduleService
{
    Task<PaginatedResultDto<StudyScheduleDto>> GetPagedAsync(StudyScheduleQueryParams query);
    Task<StudyScheduleDto?> GetByIdAsync(Guid id);
    Task<StudyScheduleDto?> CreateAsync(CreateStudyScheduleDto dto);
    Task<StudyScheduleDto?> UpdateAsync(Guid id, UpdateStudyScheduleDto dto);
    Task<bool> DeleteAsync(Guid id);
}
