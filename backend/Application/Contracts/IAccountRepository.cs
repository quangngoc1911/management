using ManagementSystem.Modules.Finance.Application.DTOs;
using ManagementSystem.Modules.Finance.Domain.Entities;

namespace ManagementSystem.Application.Contracts;

/// <summary>
/// Repository for financial accounts.
/// </summary>
public interface IAccountRepository
{
    Task<(IReadOnlyList<Account> Items, int TotalCount)> GetPagedAsync(AccountQueryParams query);
    Task<Account?> GetByIdAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task CreateAsync(Account account);
    void Update(Account account);
    Task<bool> DeleteAsync(Guid id);
}
