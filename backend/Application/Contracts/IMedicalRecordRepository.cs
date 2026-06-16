using ManagementSystem.Modules.Medical.Application.DTOs;
using ManagementSystem.Modules.Medical.Domain.Entities;

namespace ManagementSystem.Application.Contracts;

public interface IMedicalRecordRepository
{
    Task<(IReadOnlyList<MedicalRecord> Items, int TotalCount)> GetPagedAsync(MedicalRecordQueryParams query);
    Task<MedicalRecord?> GetByIdAsync(Guid id);
    Task<MedicalRecord?> GetForUpdateAsync(Guid id);
    Task CreateAsync(MedicalRecord record);
    Task<bool> DeleteAsync(Guid id);
}
