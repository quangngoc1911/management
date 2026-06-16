using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Education.Application.DTOs;

namespace ManagementSystem.Application.Contracts;

public interface IEducationRecordService
{
    Task<PaginatedResultDto<EducationRecordDto>> GetPagedAsync(EducationRecordQueryParams query);
    Task<EducationRecordDto?> GetByIdAsync(Guid id);
    Task<EducationRecordDto?> CreateAsync(CreateEducationRecordDto dto);
    Task<EducationRecordDto?> UpdateAsync(Guid id, UpdateEducationRecordDto dto);
    Task<bool> DeleteAsync(Guid id);
}
