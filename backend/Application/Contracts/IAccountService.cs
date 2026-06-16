using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Finance.Application.DTOs;

namespace ManagementSystem.Application.Contracts;

public interface IAccountService
{
    Task<PaginatedResultDto<AccountDto>> GetPagedAsync(AccountQueryParams query);
    Task<AccountDto?> GetByIdAsync(Guid id);
    Task<AccountDto> CreateAsync(CreateAccountDto dto);
    Task<AccountDto?> UpdateAsync(Guid id, UpdateAccountDto dto);
    Task<bool> DeleteAsync(Guid id);
}
