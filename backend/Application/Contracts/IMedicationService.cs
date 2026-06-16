using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Medical.Application.DTOs;

namespace ManagementSystem.Application.Contracts;

public interface IMedicationService
{
    Task<PaginatedResultDto<MedicationDto>> GetPagedAsync(MedicationQueryParams query);
    Task<MedicationDto?> GetByIdAsync(Guid id);
    Task<MedicationDto?> CreateAsync(CreateMedicationDto dto);
    Task<MedicationDto?> UpdateAsync(Guid id, UpdateMedicationDto dto);
    Task<bool> DeleteAsync(Guid id);
}
