namespace MyAPI.Models;

public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "Viewer";

    public string? AvatarUrl { get; set; }
    public string? Department { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? LastLoginAt { get; set; }

    public ICollection<Document> CreatedDocuments { get; set; } = new List<Document>();
    public ICollection<DocumentVersion> EditedVersions { get; set; } = new List<DocumentVersion>();
    public ICollection<DocumentFile> UploadedFiles { get; set; } = new List<DocumentFile>();
    public ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
    public ICollection<ViewHistory> ViewHistories { get; set; } = new List<ViewHistory>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}