using System;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.Auth.Domain.Entities;

namespace ManagementSystem.Modules.Documents.Domain.Entities;

public class Attachment : BaseEntity
{
    public Guid DocumentId { get; set; }
    public Document? Document { get; set; }

    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string FileType { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }

    public Guid UploadedByUserId { get; set; }
    public User? UploadedByUser { get; set; }
}