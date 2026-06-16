using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Finance.Application.DTOs;

namespace ManagementSystem.Application.Contracts;

public interface IRecurringTransactionService
{
    Task<PaginatedResultDto<RecurringTransactionDto>> GetPagedAsync(RecurringTransactionQueryParams query);
    Task<RecurringTransactionDto?> GetByIdAsync(Guid id);
    Task<RecurringTransactionDto?> CreateAsync(CreateRecurringTransactionDto dto);
    Task<RecurringTransactionDto?> UpdateAsync(Guid id, UpdateRecurringTransactionDto dto);
    Task<bool> DeleteAsync(Guid id);
}
