using ManagementSystem.Modules.Family.Application.DTOs;
using ManagementSystem.Modules.Family.Domain.Entities;

namespace ManagementSystem.Application.Contracts;

/// <summary>
/// Repository interface for FamilyMember operations.
/// </summary>
public interface IFamilyMemberRepository
{
    /// <summary>
    /// Get a paged + filtered list of family members.
    /// </summary>
    Task<(IReadOnlyList<FamilyMember> Items, int TotalCount)> GetPagedAsync(FamilyMemberQueryParams query);

    /// <summary>
    /// Get a single family member by id (read-only).
    /// </summary>
    Task<FamilyMember?> GetByIdAsync(Guid id);

    /// <summary>
    /// Check whether a family member exists.
    /// </summary>
    Task<bool> ExistsAsync(Guid id);

    /// <summary>
    /// Stage a new family member for insertion.
    /// </summary>
    Task CreateAsync(FamilyMember member);

    /// <summary>
    /// Stage an existing family member for update.
    /// </summary>
    void Update(FamilyMember member);

    /// <summary>
    /// Soft-delete a family member. Returns false when not found.
    /// </summary>
    Task<bool> DeleteAsync(Guid id);
}
