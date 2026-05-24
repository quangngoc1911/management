using System;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Domain.Enums;
using ManagementSystem.Modules.Auth.Domain.Entities;
using ManagementSystem.Modules.Family.Domain.Entities;

namespace ManagementSystem.Modules.Utility.Domain.Entities;

public class Reminder : BaseEntity
{
    public Guid UserId { get; set; }
    public User? User { get; set; }

    public Guid? MemberId { get; set; }
    public FamilyMember? Member { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime RemindAt { get; set; }
    public string? RecurrenceRule { get; set; }

    public string? EntityType { get; set; }
    public Guid? EntityId { get; set; }

    public new ReminderStatus Status { get; set; } = ReminderStatus.Pending;
    public DateTime? SnoozedUntil { get; set; }
}
