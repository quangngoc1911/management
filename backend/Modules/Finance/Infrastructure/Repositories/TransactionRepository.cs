using Microsoft.EntityFrameworkCore;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Infrastructure.Persistence;
using ManagementSystem.Modules.Finance.Application.DTOs;
using ManagementSystem.Modules.Finance.Domain.Entities;

namespace ManagementSystem.Modules.Finance.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationDbContext _context;

    public TransactionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    private IQueryable<Transaction> WithIncludes() =>
        _context.Transactions
            .Include(t => t.Account)
            .Include(t => t.Category)
            .Include(t => t.Member)
            .Include(t => t.TransferToAccount);

    public async Task<(IReadOnlyList<Transaction> Items, int TotalCount)> GetPagedAsync(TransactionQueryParams query)
    {
        var txQuery = WithIncludes().AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower().Trim();
            txQuery = txQuery.Where(t =>
                (t.Description != null && t.Description.ToLower().Contains(search)) ||
                (t.Note != null && t.Note.ToLower().Contains(search)));
        }

        if (query.AccountId.HasValue)
            txQuery = txQuery.Where(t => t.AccountId == query.AccountId);
        if (query.CategoryId.HasValue)
            txQuery = txQuery.Where(t => t.CategoryId == query.CategoryId);
        if (query.MemberId.HasValue)
            txQuery = txQuery.Where(t => t.MemberId == query.MemberId);
        if (!string.IsNullOrWhiteSpace(query.Type))
            txQuery = txQuery.Where(t => t.Type == query.Type);
        if (query.Status.HasValue)
            txQuery = txQuery.Where(t => t.Status == query.Status);
        if (query.FromDate.HasValue)
            txQuery = txQuery.Where(t => t.TransactionDate >= query.FromDate.Value);
        if (query.ToDate.HasValue)
            txQuery = txQuery.Where(t => t.TransactionDate <= query.ToDate.Value);

        txQuery = query.SortBy?.ToLower() switch
        {
            "amount" => query.IsDescending ? txQuery.OrderByDescending(t => t.Amount) : txQuery.OrderBy(t => t.Amount),
            "createdat" => query.IsDescending ? txQuery.OrderByDescending(t => t.CreatedAt) : txQuery.OrderBy(t => t.CreatedAt),
            _ => query.IsDescending ? txQuery.OrderByDescending(t => t.TransactionDate) : txQuery.OrderBy(t => t.TransactionDate)
        };

        var totalCount = await txQuery.CountAsync();
        var items = await txQuery
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Transaction?> GetByIdAsync(Guid id) =>
        await WithIncludes().AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);

    public async Task<Transaction?> GetForUpdateAsync(Guid id) =>
        await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id);

    public async Task CreateAsync(Transaction transaction) =>
        await _context.Transactions.AddAsync(transaction);

    public void Update(Transaction transaction) =>
        _context.Transactions.Update(transaction);

    public async Task<bool> DeleteAsync(Guid id)
    {
        var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id);
        if (transaction == null)
        {
            return false;
        }

        transaction.IsDeleted = true;
        return true;
    }
}
