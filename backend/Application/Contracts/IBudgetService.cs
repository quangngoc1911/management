using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Finance.Application.DTOs;

namespace ManagementSystem.Application.Contracts;

public interface IBudgetService
{
    Task<PaginatedResultDto<BudgetDto>> GetPagedAsync(BudgetQueryParams query);
    Task<BudgetDto?> GetByIdAsync(Guid id);
    Task<BudgetDto> CreateAsync(CreateBudgetDto dto);
    Task<BudgetDto?> UpdateAsync(Guid id, UpdateBudgetDto dto);
    Task<bool> DeleteAsync(Guid id);
}
