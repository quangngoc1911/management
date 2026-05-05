using System;
using System.Collections.Generic;

namespace ManagementSystem.Entities;

public class Document : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? DocumentNumber { get; set; }
    public string? Slug { get; set; }
    public string? Summary { get; set; }
    public string? ContentType { get; set; } // Text, File, etc.
    public string? Status { get; set; } = "Draft"; // Draft, Published, Archieved

    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }

    public DateTime? IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }

    // File properties
    public string? FilePath { get; set; }
    public string? FileName { get; set; }
    public long? FileSize { get; set; }
    public string? FileType { get; set; }
    public string? ThumbnailUrl { get; set; }

    public bool IsPublished { get; set; } = false;
    public DateTime? PublishedAt { get; set; }

    public Guid CreatedByUserId { get; set; }
    public User? CreatedByUser { get; set; }

    public Guid? UpdatedByUserId { get; set; }
    public User? UpdatedByUser { get; set; }

    // Navigation properties
    public ICollection<DocumentField> Fields { get; set; } = new List<DocumentField>();
    public ICollection<DocumentTag> DocumentTags { get; set; } = new List<DocumentTag>();
    public ICollection<DocumentVersion> Versions { get; set; } = new List<DocumentVersion>();
    public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<DocumentShare> Shares { get; set; } = new List<DocumentShare>();
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
}

public class DocumentField : BaseEntity
{
    public Guid DocumentId { get; set; }
    public Document? Document { get; set; }

    public string FieldName { get; set; } = string.Empty;
    public string FieldValue { get; set; } = string.Empty;
    public string? FieldType { get; set; } = "Text";
    public int SortOrder { get; set; }
}
