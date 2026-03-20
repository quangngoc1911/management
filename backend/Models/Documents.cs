namespace MyAPI.Models;

using System.ComponentModel.DataAnnotations;

public class Documents
{
    public int DocumentsId { get; set; }
    [StringLength(256)]
    public string? Title { get; set; }
    [StringLength(256)]
    public string? Slug { get; set; }
    [StringLength(256)]
    public string? Summary { get; set; }

    public string? ContentType { get; set; }

    public string? Content { get; set; }

    public string? FileId { get; set; }

    public string? ThumbnailUrl { get; set; }

    public int? CreatedByUserId { get; set; }
    public string? IsPublished { get; set; }

    public string? PublishedAt { get; set; }

    public int ViewCount { get; set; }

    public int SortOrder { get; set; }

    public int Version { get; set; }

    public string? CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }

    // FK → User
    public int UserId { get; set; }
    public User User { get; set; }

    // FK → Category
    public int CategoryId { get; set; }
    public Categories Category { get; set; }

    public List<DocumentVersions> Versions { get; set; }
    public List<DocumentTags> DocumentTags { get; set; }
    public List<Bookmarks> Bookmarks { get; set; }

}