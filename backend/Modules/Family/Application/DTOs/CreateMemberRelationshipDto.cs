using ManagementSystem.Domain.Enums;

namespace ManagementSystem.Modules.Family.Application.DTOs;

public class CreateMemberRelationshipDto
{
    public Guid MemberId { get; set; }
    public Guid RelatedMemberId { get; set; }
    public RelationshipType RelationshipType { get; set; }
    public DateOnly? StartedAt { get; set; }
    public DateOnly? EndedAt { get; set; }
    public bool? IsBiological { get; set; }
    public string? Notes { get; set; }
}
