using ManagementSystem.Modules.Finance.Application.DTOs;
using ManagementSystem.Modules.Finance.Domain.Entities;

namespace ManagementSystem.Application.Contracts;

public interface IRecurringTransactionRepository
{
    Task<(IReadOnlyList<RecurringTransaction> Items, int TotalCount)> GetPagedAsync(RecurringTransactionQueryParams query);
    Task<RecurringTransaction?> GetByIdAsync(Guid id);
    Task<RecurringTransaction?> GetForUpdateAsync(Guid id);
    Task CreateAsync(RecurringTransaction recurring);
    Task<bool> DeleteAsync(Guid id);
}
