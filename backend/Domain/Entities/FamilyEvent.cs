using System;
using System.Collections.Generic;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Domain.Enums;
using ManagementSystem.Modules.Auth.Domain.Entities;
using ManagementSystem.Modules.Documents.Domain.Entities;

namespace ManagementSystem.Modules.Events.Domain.Entities;

public class FamilyEvent : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? EventType { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    public bool AllDay { get; set; }
    public string? Location { get; set; }
    public string? RecurrenceRule { get; set; }
    public new EventStatus Status { get; set; } = EventStatus.Planned;

    public Guid? CoverFileId { get; set; }
    public DocumentFile? CoverFile { get; set; }

    public Guid CreatedByUserId { get; set; }
    public User? CreatedByUser { get; set; }

    public ICollection<EventMedia> Media { get; set; } = new List<EventMedia>();
}
