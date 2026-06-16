using ManagementSystem.Modules.Finance.Application.DTOs;
using ManagementSystem.Modules.Finance.Domain.Entities;

namespace ManagementSystem.Application.Contracts;

public interface IInvestmentRepository
{
    Task<(IReadOnlyList<Investment> Items, int TotalCount)> GetPagedAsync(InvestmentQueryParams query);
    Task<Investment?> GetByIdAsync(Guid id);
    Task<Investment?> GetForUpdateAsync(Guid id);
    Task CreateAsync(Investment investment);
    Task<bool> DeleteAsync(Guid id);
}
