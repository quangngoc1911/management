using Microsoft.EntityFrameworkCore;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Infrastructure.Persistence;
using ManagementSystem.Modules.Finance.Application.DTOs;
using ManagementSystem.Modules.Finance.Domain.Entities;

namespace ManagementSystem.Modules.Finance.Infrastructure.Repositories;

public class RecurringTransactionRepository : IRecurringTransactionRepository
{
    private readonly ApplicationDbContext _context;

    public RecurringTransactionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyList<RecurringTransaction> Items, int TotalCount)> GetPagedAsync(RecurringTransactionQueryParams query)
    {
        var q = _context.RecurringTransactions
            .AsNoTracking()
            .Include(r => r.Account)
            .Include(r => r.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower().Trim();
            q = q.Where(r => r.Name.ToLower().Contains(search));
        }
        if (query.AccountId.HasValue)
            q = q.Where(r => r.AccountId == query.AccountId);
        if (!string.IsNullOrWhiteSpace(query.Type))
            q = q.Where(r => r.Type == query.Type);
        if (!string.IsNullOrWhiteSpace(query.Frequency))
            q = q.Where(r => r.Frequency == query.Frequency);
        if (query.IsActive.HasValue)
            q = q.Where(r => r.IsActive == query.IsActive);

        q = query.SortBy?.ToLower() switch
        {
            "name" => query.IsDescending ? q.OrderByDescending(r => r.Name) : q.OrderBy(r => r.Name),
            "amount" => query.IsDescending ? q.OrderByDescending(r => r.Amount) : q.OrderBy(r => r.Amount),
            _ => query.IsDescending ? q.OrderByDescending(r => r.NextDueDate) : q.OrderBy(r => r.NextDueDate)
        };

        var totalCount = await q.CountAsync();
        var items = await q.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToListAsync();
        return (items, totalCount);
    }

    public async Task<RecurringTransaction?> GetByIdAsync(Guid id) =>
        await _context.RecurringTransactions.AsNoTracking().Include(r => r.Account).Include(r => r.Category).FirstOrDefaultAsync(r => r.Id == id);

    public async Task<RecurringTransaction?> GetForUpdateAsync(Guid id) =>
        await _context.RecurringTransactions.FirstOrDefaultAsync(r => r.Id == id);

    public async Task CreateAsync(RecurringTransaction recurring) => await _context.RecurringTransactions.AddAsync(recurring);

    public async Task<bool> DeleteAsync(Guid id)
    {
        var recurring = await _context.RecurringTransactions.FirstOrDefaultAsync(r => r.Id == id);
        if (recurring == null) return false;
        recurring.IsDeleted = true;
        return true;
    }
}
