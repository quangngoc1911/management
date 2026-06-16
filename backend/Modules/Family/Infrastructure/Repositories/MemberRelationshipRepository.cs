using Microsoft.EntityFrameworkCore;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Domain.Enums;
using ManagementSystem.Infrastructure.Persistence;
using ManagementSystem.Modules.Family.Domain.Entities;

namespace ManagementSystem.Modules.Family.Infrastructure.Repositories;

public class MemberRelationshipRepository : IMemberRelationshipRepository
{
    private readonly ApplicationDbContext _context;

    public MemberRelationshipRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<MemberRelationship>> GetByMemberIdAsync(Guid memberId)
    {
        return await _context.MemberRelationships
            .AsNoTracking()
            .Include(r => r.Member)
            .Include(r => r.RelatedMember)
            .Where(r => r.MemberId == memberId)
            .OrderBy(r => r.RelationshipType)
            .ToListAsync();
    }

    public async Task<MemberRelationship?> GetByIdAsync(Guid id)
    {
        return await _context.MemberRelationships
            .AsNoTracking()
            .Include(r => r.Member)
            .Include(r => r.RelatedMember)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<bool> TripleExistsAsync(Guid memberId, Guid relatedMemberId, RelationshipType type, Guid? excludeId = null)
    {
        var query = _context.MemberRelationships
            .Where(r => r.MemberId == memberId && r.RelatedMemberId == relatedMemberId && r.RelationshipType == type);
        if (excludeId.HasValue)
        {
            query = query.Where(r => r.Id != excludeId.Value);
        }
        return await query.AnyAsync();
    }

    public async Task CreateAsync(MemberRelationship relationship)
    {
        await _context.MemberRelationships.AddAsync(relationship);
    }

    public void Update(MemberRelationship relationship)
    {
        _context.MemberRelationships.Update(relationship);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var relationship = await _context.MemberRelationships.FirstOrDefaultAsync(r => r.Id == id);
        if (relationship == null)
        {
            return false;
        }

        relationship.IsDeleted = true;
        _context.MemberRelationships.Update(relationship);
        return true;
    }
}
