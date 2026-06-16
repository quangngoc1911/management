using ManagementSystem.Modules.Family.Application.DTOs;

namespace ManagementSystem.Application.Contracts;

/// <summary>
/// Application service for family member relationships.
/// </summary>
public interface IMemberRelationshipService
{
    /// <summary>List a member's relationships. Returns null when the member does not exist.</summary>
    Task<List<MemberRelationshipDto>?> GetByMemberAsync(Guid memberId);

    /// <summary>Create a relationship. Returns null when a referenced member does not exist.</summary>
    Task<MemberRelationshipDto?> CreateAsync(CreateMemberRelationshipDto dto);

    Task<MemberRelationshipDto?> UpdateAsync(Guid id, UpdateMemberRelationshipDto dto);

    Task<bool> DeleteAsync(Guid id);
}
