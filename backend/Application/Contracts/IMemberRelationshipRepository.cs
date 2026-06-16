using ManagementSystem.Domain.Enums;
using ManagementSystem.Modules.Family.Domain.Entities;

namespace ManagementSystem.Application.Contracts;

/// <summary>
/// Repository for relationships between family members.
/// </summary>
public interface IMemberRelationshipRepository
{
    /// <summary>List relationships where the given member is the source, including both endpoints.</summary>
    Task<IReadOnlyList<MemberRelationship>> GetByMemberIdAsync(Guid memberId);

    Task<MemberRelationship?> GetByIdAsync(Guid id);

    Task<bool> TripleExistsAsync(Guid memberId, Guid relatedMemberId, RelationshipType type, Guid? excludeId = null);

    Task CreateAsync(MemberRelationship relationship);

    void Update(MemberRelationship relationship);

    Task<bool> DeleteAsync(Guid id);
}
