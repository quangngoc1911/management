namespace MyAPI.Models;

public class Document : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Summary { get; set; }

    // "text" hoặc "file"
    public string ContentType { get; set; } = "text";
    public string? Content { get; set; }

    public int? FileId { get; set; }
    public DocumentFile? File { get; set; }

    public string? ThumbnailUrl { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public int CreatedByUserId { get; set; }
    public User CreatedByUser { get; set; } = null!;

    public bool IsPublished { get; set; } = false;
    public DateTime? PublishedAt { get; set; }

    public int ViewCount { get; set; } = 0;
    public int SortOrder { get; set; } = 0;
    public int Version { get; set; } = 1;

    public ICollection<DocumentTag> DocumentTags { get; set; } = new List<DocumentTag>();
    public ICollection<DocumentVersion> Versions { get; set; } = new List<DocumentVersion>();
    public ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
    public ICollection<ViewHistory> ViewHistories { get; set; } = new List<ViewHistory>();
}