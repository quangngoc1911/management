using ManagementSystem.Modules.Family.Domain.Entities;

namespace ManagementSystem.Application.Contracts;

/// <summary>
/// Repository for the 1:1 detailed profile of a family member.
/// </summary>
public interface IMemberProfileRepository
{
    Task<MemberProfile?> GetByMemberIdAsync(Guid memberId);
    Task CreateAsync(MemberProfile profile);
    void Update(MemberProfile profile);
}
