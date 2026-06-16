using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Finance.Application.DTOs;

namespace ManagementSystem.Application.Contracts;

public interface IInvestmentService
{
    Task<PaginatedResultDto<InvestmentDto>> GetPagedAsync(InvestmentQueryParams query);
    Task<InvestmentDto?> GetByIdAsync(Guid id);
    Task<InvestmentDto> CreateAsync(CreateInvestmentDto dto);
    Task<InvestmentDto?> UpdateAsync(Guid id, UpdateInvestmentDto dto);
    Task<bool> DeleteAsync(Guid id);
}
