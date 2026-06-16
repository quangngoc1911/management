using ManagementSystem.Modules.Medical.Application.DTOs;
using ManagementSystem.Modules.Medical.Domain.Entities;

namespace ManagementSystem.Application.Contracts;

public interface IMedicationRepository
{
    Task<(IReadOnlyList<Medication> Items, int TotalCount)> GetPagedAsync(MedicationQueryParams query);
    Task<Medication?> GetByIdAsync(Guid id);
    Task<Medication?> GetForUpdateAsync(Guid id);
    Task CreateAsync(Medication medication);
    Task<bool> DeleteAsync(Guid id);
}
