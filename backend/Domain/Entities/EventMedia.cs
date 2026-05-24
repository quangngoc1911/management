using System;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.Auth.Domain.Entities;
using ManagementSystem.Modules.Documents.Domain.Entities;

namespace ManagementSystem.Modules.Events.Domain.Entities;

public class EventMedia : BaseEntity
{
    public Guid EventId { get; set; }
    public FamilyEvent? Event { get; set; }

    public Guid FileId { get; set; }
    public DocumentFile? File { get; set; }

    public string? Caption { get; set; }
    public short? SortOrder { get; set; }

    public Guid? UploadedByUserId { get; set; }
    public User? UploadedByUser { get; set; }
}
