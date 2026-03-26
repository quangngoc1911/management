namespace MyAPI.Models;

public class DocumentVersion : BaseEntity
{
    public int DocumentId { get; set; }
    public Document Document { get; set; } = null!;

    public int VersionNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; }
    public string? ChangeSummary { get; set; }

    public int EditedByUserId { get; set; }
    public User EditedByUser { get; set; } = null!;
}