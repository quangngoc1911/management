using System;
using System.Collections.Generic;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.Auth.Domain.Entities;
using ManagementSystem.Modules.Categories.Domain.Entities;
using ManagementSystem.Modules.Family.Domain.Entities;

namespace ManagementSystem.Modules.Documents.Domain.Entities;

public class Document : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Summary { get; set; }

    public string ContentType { get; set; } = "text";
    public string? Content { get; set; }

    public Guid? FileId { get; set; }
    public DocumentFile? File { get; set; }

    public string? ThumbnailUrl { get; set; }

    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }

    public Guid? MemberId { get; set; }
    public FamilyMember? Member { get; set; }

    public Guid CreatedByUserId { get; set; }
    public User? CreatedByUser { get; set; }

    public bool IsPublished { get; set; }
    public DateTime? PublishedAt { get; set; }
    public int ViewCount { get; set; }
    public int SortOrder { get; set; }
    public int Version { get; set; } = 1;

    public ICollection<DocumentTag> DocumentTags { get; set; } = new List<DocumentTag>();
    public ICollection<DocumentVersion> Versions { get; set; } = new List<DocumentVersion>();
}
