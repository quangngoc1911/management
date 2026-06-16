using Microsoft.EntityFrameworkCore;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Infrastructure.Persistence;
using ManagementSystem.Modules.Finance.Application.DTOs;
using ManagementSystem.Modules.Finance.Domain.Entities;

namespace ManagementSystem.Modules.Finance.Infrastructure.Repositories;

public class BudgetRepository : IBudgetRepository
{
    private readonly ApplicationDbContext _context;

    public BudgetRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyList<Budget> Items, int TotalCount)> GetPagedAsync(BudgetQueryParams query)
    {
        var budgetsQuery = _context.Budgets
            .AsNoTracking()
            .Include(b => b.Category)
            .Include(b => b.Member)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower().Trim();
            budgetsQuery = budgetsQuery.Where(b => b.Name.ToLower().Contains(search));
        }
        if (query.CategoryId.HasValue)
            budgetsQuery = budgetsQuery.Where(b => b.CategoryId == query.CategoryId);
        if (query.MemberId.HasValue)
            budgetsQuery = budgetsQuery.Where(b => b.MemberId == query.MemberId);
        if (!string.IsNullOrWhiteSpace(query.PeriodType))
            budgetsQuery = budgetsQuery.Where(b => b.PeriodType == query.PeriodType);
        if (query.IsActive.HasValue)
            budgetsQuery = budgetsQuery.Where(b => b.IsActive == query.IsActive);

        budgetsQuery = query.SortBy?.ToLower() switch
        {
            "amount" => query.IsDescending ? budgetsQuery.OrderByDescending(b => b.Amount) : budgetsQuery.OrderBy(b => b.Amount),
            "startdate" => query.IsDescending ? budgetsQuery.OrderByDescending(b => b.StartDate) : budgetsQuery.OrderBy(b => b.StartDate),
            _ => query.IsDescending ? budgetsQuery.OrderByDescending(b => b.Name) : budgetsQuery.OrderBy(b => b.Name)
        };

        var totalCount = await budgetsQuery.CountAsync();
        var items = await budgetsQuery.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToListAsync();
        return (items, totalCount);
    }

    public async Task<Budget?> GetByIdAsync(Guid id) =>
        await _context.Budgets.AsNoTracking().Include(b => b.Category).Include(b => b.Member).FirstOrDefaultAsync(b => b.Id == id);

    public async Task<Budget?> GetForUpdateAsync(Guid id) =>
        await _context.Budgets.FirstOrDefaultAsync(b => b.Id == id);

    public async Task CreateAsync(Budget budget) => await _context.Budgets.AddAsync(budget);

    public async Task<bool> DeleteAsync(Guid id)
    {
        var budget = await _context.Budgets.FirstOrDefaultAsync(b => b.Id == id);
        if (budget == null) return false;
        budget.IsDeleted = true;
        return true;
    }
}
