using System;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.Auth.Domain.Entities;

namespace ManagementSystem.Modules.Documents.Domain.Entities;

public class DocumentVersion : BaseEntity
{
    public Guid DocumentId { get; set; }
    public Document? Document { get; set; }

    public int VersionNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public string? ChangeSummary { get; set; }

    public Guid EditedByUserId { get; set; }
    public User? EditedByUser { get; set; }
}
