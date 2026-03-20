namespace MyAPI.Models;

using System.ComponentModel.DataAnnotations;

public class Files
{
    public int FilesId { get; set; }
    [StringLength(256)]
    public string? OriginalName { get; set; }
    [StringLength(256)]
    public string? StoredName { get; set; }
    public int StoragePath { get; set; }
    [StringLength(256)]
    public string? PublicUrl { get; set; }
    [StringLength(256)]
    public string? FileType { get; set; }
    public int MimeType { get; set; }
    [StringLength(256)]
    public string? SizeBytes { get; set; }
    [StringLength(256)]
    public string? UploadedByUserId { get; set; }
    public string? StorageProvider { get; set; }
    [StringLength(256)]
    public string? CreatedAt { get; set; }

    public int DocumentId { get; set; } // FK (unique)
    public Documents Document { get; set; }

    public string FilePath { get; set; }

}