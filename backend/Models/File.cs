namespace MyAPI.Models;

using System.ComponentModel.DataAnnotations;

public class File
{
    public int FilesId { get; set; }
    public string? OriginalName { get; set; }
    public string? StoredName { get; set; }
    public int StoragePath { get; set; }
    public string? PublicUrl { get; set; }
    public string? FileType { get; set; }
    public int MimeType { get; set; }
    public string? SizeBytes { get; set; }
    public string? UploadedByUserId { get; set; }
    public string? StorageProvider { get; set; }
    public string? CreatedAt { get; set; }
    public int DocumentId { get; set; }
    public List<Document> Documents { get; set; } = new();
    public string FilePath { get; set; } = string.Empty;

}