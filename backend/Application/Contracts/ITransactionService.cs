using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Finance.Application.DTOs;

namespace ManagementSystem.Application.Contracts;

public interface ITransactionService
{
    Task<PaginatedResultDto<TransactionDto>> GetPagedAsync(TransactionQueryParams query);
    Task<TransactionDto?> GetByIdAsync(Guid id);
    Task<TransactionDto?> CreateAsync(CreateTransactionDto dto);
    Task<TransactionDto?> UpdateAsync(Guid id, UpdateTransactionDto dto);
    Task<bool> DeleteAsync(Guid id);
}
