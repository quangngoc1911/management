using Microsoft.EntityFrameworkCore;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Infrastructure.Persistence;
using ManagementSystem.Modules.Finance.Application.DTOs;
using ManagementSystem.Modules.Finance.Domain.Entities;

namespace ManagementSystem.Modules.Finance.Infrastructure.Repositories;

public class InvestmentRepository : IInvestmentRepository
{
    private readonly ApplicationDbContext _context;

    public InvestmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyList<Investment> Items, int TotalCount)> GetPagedAsync(InvestmentQueryParams query)
    {
        var q = _context.Investments
            .AsNoTracking()
            .Include(i => i.Account)
            .Include(i => i.Member)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower().Trim();
            q = q.Where(i => i.Name.ToLower().Contains(search) || (i.Symbol != null && i.Symbol.ToLower().Contains(search)));
        }
        if (query.AccountId.HasValue)
            q = q.Where(i => i.AccountId == query.AccountId);
        if (query.MemberId.HasValue)
            q = q.Where(i => i.MemberId == query.MemberId);
        if (!string.IsNullOrWhiteSpace(query.Type))
            q = q.Where(i => i.Type == query.Type);
        if (query.IsActive.HasValue)
            q = q.Where(i => i.IsActive == query.IsActive);

        q = query.SortBy?.ToLower() switch
        {
            "type" => query.IsDescending ? q.OrderByDescending(i => i.Type) : q.OrderBy(i => i.Type),
            _ => query.IsDescending ? q.OrderByDescending(i => i.Name) : q.OrderBy(i => i.Name)
        };

        var totalCount = await q.CountAsync();
        var items = await q.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToListAsync();
        return (items, totalCount);
    }

    public async Task<Investment?> GetByIdAsync(Guid id) =>
        await _context.Investments.AsNoTracking().Include(i => i.Account).Include(i => i.Member).FirstOrDefaultAsync(i => i.Id == id);

    public async Task<Investment?> GetForUpdateAsync(Guid id) =>
        await _context.Investments.FirstOrDefaultAsync(i => i.Id == id);

    public async Task CreateAsync(Investment investment) => await _context.Investments.AddAsync(investment);

    public async Task<bool> DeleteAsync(Guid id)
    {
        var investment = await _context.Investments.FirstOrDefaultAsync(i => i.Id == id);
        if (investment == null) return false;
        investment.IsDeleted = true;
        return true;
    }
}
