using ManagementSystem.Domain.Enums;

namespace ManagementSystem.Modules.Utility.Application.DTOs;

public class CreateReminderDto
{
    public Guid? MemberId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime RemindAt { get; set; }
    public string? RecurrenceRule { get; set; }
    public string? EntityType { get; set; }
    public Guid? EntityId { get; set; }
    public ReminderStatus Status { get; set; } = ReminderStatus.Pending;
    public DateTime? SnoozedUntil { get; set; }

    /// <summary>Set by the controller from the authenticated user.</summary>
    public Guid UserId { get; set; }
}
