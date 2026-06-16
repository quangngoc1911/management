using ManagementSystem.Domain.Enums;

namespace ManagementSystem.Modules.Family.Application.DTOs;

/// <summary>
/// Request payload to update an existing family member (full replace semantics).
/// </summary>
public class UpdateFamilyMemberDto
{
    public Guid? UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Nickname { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public DateOnly? DateOfDeath { get; set; }
    public Gender? Gender { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public RelationToHead? RelationToHead { get; set; }
    public bool IsHouseholdHead { get; set; }
    public string? Notes { get; set; }

    /// <summary>Set by the controller from the authenticated user; not supplied by the client.</summary>
    public Guid? UpdatedBy { get; set; }
}
