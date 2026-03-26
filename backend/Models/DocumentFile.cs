namespace MyAPI.Models;

public class DocumentFile : BaseEntity
{
    public string OriginalName { get; set; } = string.Empty;
    public string StoredName { get; set; } = string.Empty;
    public string StoragePath { get; set; } = string.Empty;
    public string PublicUrl { get; set; } = string.Empty;

    public string FileType { get; set; } = string.Empty;     // pdf/docx/doc/jpg/png/mp4...
    public string MimeType { get; set; } = string.Empty;
    public long SizeBytes { get; set; }

    public int UploadedByUserId { get; set; }
    public User UploadedByUser { get; set; } = null!;

    public string StorageProvider { get; set; } = "local";

    public ICollection<Document> Documents { get; set; } = new List<Document>();
}