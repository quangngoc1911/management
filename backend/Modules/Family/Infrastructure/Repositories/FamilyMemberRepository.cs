using Microsoft.EntityFrameworkCore;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Infrastructure.Persistence;
using ManagementSystem.Modules.Family.Application.DTOs;
using ManagementSystem.Modules.Family.Domain.Entities;

namespace ManagementSystem.Modules.Family.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for FamilyMember operations.
/// Soft-deleted rows are excluded automatically by the global query filter.
/// </summary>
public class FamilyMemberRepository : IFamilyMemberRepository
{
    private readonly ApplicationDbContext _context;

    public FamilyMemberRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyList<FamilyMember> Items, int TotalCount)> GetPagedAsync(FamilyMemberQueryParams query)
    {
        var membersQuery = _context.FamilyMembers
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower().Trim();
            membersQuery = membersQuery.Where(m =>
                m.FullName.ToLower().Contains(search) ||
                (m.Nickname != null && m.Nickname.ToLower().Contains(search)) ||
                (m.Phone != null && m.Phone.Contains(search)) ||
                (m.Email != null && m.Email.ToLower().Contains(search)));
        }

        if (query.Gender.HasValue)
        {
            membersQuery = membersQuery.Where(m => m.Gender == query.Gender);
        }

        if (query.RelationToHead.HasValue)
        {
            membersQuery = membersQuery.Where(m => m.RelationToHead == query.RelationToHead);
        }

        if (query.IsHouseholdHead.HasValue)
        {
            membersQuery = membersQuery.Where(m => m.IsHouseholdHead == query.IsHouseholdHead);
        }

        membersQuery = query.SortBy?.ToLower() switch
        {
            "dateofbirth" => query.IsDescending
                ? membersQuery.OrderByDescending(m => m.DateOfBirth)
                : membersQuery.OrderBy(m => m.DateOfBirth),
            "createdat" => query.IsDescending
                ? membersQuery.OrderByDescending(m => m.CreatedAt)
                : membersQuery.OrderBy(m => m.CreatedAt),
            _ => query.IsDescending
                ? membersQuery.OrderByDescending(m => m.FullName)
                : membersQuery.OrderBy(m => m.FullName)
        };

        var totalCount = await membersQuery.CountAsync();
        var items = await membersQuery
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<FamilyMember?> GetByIdAsync(Guid id)
    {
        return await _context.FamilyMembers
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.FamilyMembers.AnyAsync(m => m.Id == id);
    }

    public async Task CreateAsync(FamilyMember member)
    {
        await _context.FamilyMembers.AddAsync(member);
    }

    public void Update(FamilyMember member)
    {
        _context.FamilyMembers.Update(member);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var member = await _context.FamilyMembers.FirstOrDefaultAsync(m => m.Id == id);
        if (member == null)
        {
            return false;
        }

        // Soft delete: the SaveChanges audit pipeline keeps DeletedAt/IsDeleted consistent.
        member.IsDeleted = true;
        _context.FamilyMembers.Update(member);
        return true;
    }
}
