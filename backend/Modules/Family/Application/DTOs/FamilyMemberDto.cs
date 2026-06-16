using ManagementSystem.Domain.Enums;

namespace ManagementSystem.Modules.Family.Application.DTOs;

/// <summary>
/// Read model for a family member (used for both list and detail responses).
/// </summary>
public class FamilyMemberDto
{
    public Guid Id { get; set; }
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
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
