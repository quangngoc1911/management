using System;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Domain.Enums;

namespace ManagementSystem.Modules.Family.Domain.Entities;

public class MemberRelationship : BaseEntity
{
    public Guid MemberId { get; set; }
    public FamilyMember? Member { get; set; }

    public Guid RelatedMemberId { get; set; }
    public FamilyMember? RelatedMember { get; set; }

    public RelationshipType RelationshipType { get; set; }
    public DateOnly? StartedAt { get; set; }
    public DateOnly? EndedAt { get; set; }
    public bool? IsBiological { get; set; }
    public string? Notes { get; set; }
}
