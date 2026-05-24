using System;
using System.Collections.Generic;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Domain.Enums;
using ManagementSystem.Modules.Auth.Domain.Entities;
using ManagementSystem.Modules.Documents.Domain.Entities;

namespace ManagementSystem.Modules.Family.Domain.Entities;

public class FamilyMember : BaseEntity
{
    public Guid? UserId { get; set; }
    public User? User { get; set; }

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

    public MemberProfile? Profile { get; set; }
    public ICollection<MemberRelationship> RelationshipsAsSource { get; set; } = new List<MemberRelationship>();
    public ICollection<MemberRelationship> RelationshipsAsTarget { get; set; } = new List<MemberRelationship>();

    public ICollection<Document> Documents { get; set; } = new List<Document>();
}
