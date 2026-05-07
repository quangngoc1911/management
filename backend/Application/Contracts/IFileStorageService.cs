namespace ManagementSystem.Application.Contracts;

/// <summary>
/// Service interface for file storage operations
/// </summary>
public interface IFileStorageService
{
    /// <summary>
    /// Upload a file and return storage info
    /// </summary>
    Task<FileUploadResult> UploadAsync(IFormFile file, string folder);
    
    /// <summary>
    /// Download a file from storage
    /// </summary>
    Task<byte[]?> DownloadAsync(string filePath);
    
    /// <summary>
    /// Delete a file from storage
    /// </summary>
    Task DeleteAsync(string filePath);
    
    /// <summary>
    /// Get file URL for access
    /// </summary>
    string GetFileUrl(string filePath);
}

/// <summary>
/// Result of file upload operation
/// </summary>
public class FileUploadResult
{
    public string FilePath { get; set; } = string.Empty;
    public string OriginalFileName { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string ContentType { get; set; } = string.Empty;
}