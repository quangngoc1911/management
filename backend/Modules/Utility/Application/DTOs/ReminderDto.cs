using ManagementSystem.Domain.Enums;

namespace ManagementSystem.Modules.Utility.Application.DTOs;

public class ReminderDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? MemberId { get; set; }
    public string? MemberName { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime RemindAt { get; set; }
    public string? RecurrenceRule { get; set; }
    public string? EntityType { get; set; }
    public Guid? EntityId { get; set; }
    public ReminderStatus Status { get; set; }
    public DateTime? SnoozedUntil { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
