using ManagementSystem.Modules.Finance.Application.DTOs;
using ManagementSystem.Modules.Finance.Domain.Entities;

namespace ManagementSystem.Application.Contracts;

public interface IBudgetRepository
{
    Task<(IReadOnlyList<Budget> Items, int TotalCount)> GetPagedAsync(BudgetQueryParams query);
    Task<Budget?> GetByIdAsync(Guid id);
    Task<Budget?> GetForUpdateAsync(Guid id);
    Task CreateAsync(Budget budget);
    Task<bool> DeleteAsync(Guid id);
}
