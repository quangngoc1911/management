using ManagementSystem.Modules.Finance.Application.DTOs;
using ManagementSystem.Modules.Finance.Domain.Entities;

namespace ManagementSystem.Application.Contracts;

public interface ITransactionRepository
{
    Task<(IReadOnlyList<Transaction> Items, int TotalCount)> GetPagedAsync(TransactionQueryParams query);
    Task<Transaction?> GetByIdAsync(Guid id);
    /// <summary>Tracked fetch without navigation includes, for safe scalar updates.</summary>
    Task<Transaction?> GetForUpdateAsync(Guid id);
    Task CreateAsync(Transaction transaction);
    void Update(Transaction transaction);
    Task<bool> DeleteAsync(Guid id);
}
