using Microsoft.EntityFrameworkCore;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Infrastructure.Persistence;
using ManagementSystem.Modules.Finance.Application.DTOs;
using ManagementSystem.Modules.Finance.Domain.Entities;

namespace ManagementSystem.Modules.Finance.Infrastructure.Repositories;

/// <summary>
/// Repository for financial accounts. AccountNumber is encrypted at rest, so it is not searchable.
/// </summary>
public class AccountRepository : IAccountRepository
{
    private readonly ApplicationDbContext _context;

    public AccountRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyList<Account> Items, int TotalCount)> GetPagedAsync(AccountQueryParams query)
    {
        var accountsQuery = _context.Accounts
            .AsNoTracking()
            .Include(a => a.Member)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower().Trim();
            accountsQuery = accountsQuery.Where(a =>
                a.Name.ToLower().Contains(search) ||
                (a.BankName != null && a.BankName.ToLower().Contains(search)));
        }

        if (query.MemberId.HasValue)
        {
            accountsQuery = accountsQuery.Where(a => a.MemberId == query.MemberId);
        }

        if (!string.IsNullOrWhiteSpace(query.AccountType))
        {
            accountsQuery = accountsQuery.Where(a => a.AccountType == query.AccountType);
        }

        if (query.IsActive.HasValue)
        {
            accountsQuery = accountsQuery.Where(a => a.IsActive == query.IsActive);
        }

        accountsQuery = query.SortBy?.ToLower() switch
        {
            "createdat" => query.IsDescending
                ? accountsQuery.OrderByDescending(a => a.CreatedAt)
                : accountsQuery.OrderBy(a => a.CreatedAt),
            _ => query.IsDescending
                ? accountsQuery.OrderByDescending(a => a.Name)
                : accountsQuery.OrderBy(a => a.Name)
        };

        var totalCount = await accountsQuery.CountAsync();
        var items = await accountsQuery
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Account?> GetByIdAsync(Guid id)
    {
        return await _context.Accounts
            .AsNoTracking()
            .Include(a => a.Member)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Accounts.AnyAsync(a => a.Id == id);
    }

    public async Task CreateAsync(Account account)
    {
        await _context.Accounts.AddAsync(account);
    }

    public void Update(Account account)
    {
        _context.Accounts.Update(account);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == id);
        if (account == null)
        {
            return false;
        }

        account.IsDeleted = true;
        _context.Accounts.Update(account);
        return true;
    }
}
