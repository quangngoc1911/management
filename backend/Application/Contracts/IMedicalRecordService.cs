using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Medical.Application.DTOs;

namespace ManagementSystem.Application.Contracts;

public interface IMedicalRecordService
{
    Task<PaginatedResultDto<MedicalRecordDto>> GetPagedAsync(MedicalRecordQueryParams query);
    Task<MedicalRecordDto?> GetByIdAsync(Guid id);
    Task<MedicalRecordDto?> CreateAsync(CreateMedicalRecordDto dto);
    Task<MedicalRecordDto?> UpdateAsync(Guid id, UpdateMedicalRecordDto dto);
    Task<bool> DeleteAsync(Guid id);
}
