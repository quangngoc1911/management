using ManagementSystem.Domain.Enums;

namespace ManagementSystem.Modules.Events.Application.DTOs;

public class FamilyEventDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? EventType { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    public bool AllDay { get; set; }
    public string? Location { get; set; }
    public string? RecurrenceRule { get; set; }
    public EventStatus Status { get; set; }
    public Guid? CoverFileId { get; set; }
    public Guid CreatedByUserId { get; set; }
    public string? CreatedByUserName { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
