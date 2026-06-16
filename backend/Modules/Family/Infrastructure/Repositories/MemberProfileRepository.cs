using Microsoft.EntityFrameworkCore;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Infrastructure.Persistence;
using ManagementSystem.Modules.Family.Domain.Entities;

namespace ManagementSystem.Modules.Family.Infrastructure.Repositories;

/// <summary>
/// Repository for MemberProfile (1:1 with FamilyMember).
/// Encrypted columns (NationalId/PassportNo) are handled transparently by the EF value converter.
/// </summary>
public class MemberProfileRepository : IMemberProfileRepository
{
    private readonly ApplicationDbContext _context;

    public MemberProfileRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MemberProfile?> GetByMemberIdAsync(Guid memberId)
    {
        return await _context.MemberProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.MemberId == memberId);
    }

    public async Task CreateAsync(MemberProfile profile)
    {
        await _context.MemberProfiles.AddAsync(profile);
    }

    public void Update(MemberProfile profile)
    {
        _context.MemberProfiles.Update(profile);
    }
}
