using ManagementSystem.Modules.Family.Application.DTOs;

namespace ManagementSystem.Application.Contracts;

/// <summary>
/// Application service for a family member's detailed profile (1:1).
/// </summary>
public interface IMemberProfileService
{
    /// <summary>Get the profile for a member, or null when the member or profile does not exist.</summary>
    Task<MemberProfileDto?> GetByMemberIdAsync(Guid memberId);

    /// <summary>Create or update the profile. Returns null when the member does not exist.</summary>
    Task<MemberProfileDto?> UpsertAsync(Guid memberId, UpsertMemberProfileDto dto);
}
