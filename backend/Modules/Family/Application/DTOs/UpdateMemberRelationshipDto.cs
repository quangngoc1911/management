using ManagementSystem.Domain.Enums;

namespace ManagementSystem.Modules.Family.Application.DTOs;

public class UpdateMemberRelationshipDto
{
    public RelationshipType RelationshipType { get; set; }
    public DateOnly? StartedAt { get; set; }
    public DateOnly? EndedAt { get; set; }
    public bool? IsBiological { get; set; }
    public string? Notes { get; set; }
}
