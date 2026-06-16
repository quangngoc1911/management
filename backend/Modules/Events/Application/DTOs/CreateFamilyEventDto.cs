using ManagementSystem.Domain.Enums;

namespace ManagementSystem.Modules.Events.Application.DTOs;

public class CreateFamilyEventDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? EventType { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    public bool AllDay { get; set; }
    public string? Location { get; set; }
    public string? RecurrenceRule { get; set; }
    public EventStatus Status { get; set; } = EventStatus.Planned;
    public Guid? CoverFileId { get; set; }

    /// <summary>Set by the controller from the authenticated user.</summary>
    public Guid CreatedByUserId { get; set; }
}
