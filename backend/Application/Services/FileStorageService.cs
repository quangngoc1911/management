using ManagementSystem.Application.Contracts;

namespace ManagementSystem.Application.Services;

/// <summary>
/// Service implementation for file storage operations
/// </summary>
public class FileStorageService : IFileStorageService
{
    private readonly string _basePath;
    private readonly ILogger<FileStorageService> _logger;
    
    public FileStorageService(ILogger<FileStorageService> logger, IConfiguration? configuration = null)
    {
        _logger = logger;
        _basePath = configuration?.GetValue<string>("FileStorage:BasePath") ?? "uploads";
        
        if (!Directory.Exists(_basePath))
        {
            Directory.CreateDirectory(_basePath);
        }
    }
    
    public async Task<FileUploadResult> UploadAsync(IFormFile file, string folder)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("No file provided");
        }
        
        // Create folder path
        var folderPath = Path.Combine(_basePath, folder);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        
        // Generate unique file name
        var originalFileName = file.FileName;
        var extension = Path.GetExtension(originalFileName);
        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(folderPath, fileName);
        
        // Save file
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        
        return new FileUploadResult
        {
            FilePath = Path.Combine(folder, fileName),
            OriginalFileName = originalFileName,
            FileSize = file.Length,
            ContentType = file.ContentType
        };
    }
    
    public Task<byte[]?> DownloadAsync(string filePath)
    {
        var fullPath = Path.Combine(_basePath, filePath);
        
        if (!File.Exists(fullPath))
        {
            return Task.FromResult<byte[]?>(null);
        }
        
        return Task.FromResult<byte[]?>(File.ReadAllBytes(fullPath));
    }
    
    public Task DeleteAsync(string filePath)
    {
        var fullPath = Path.Combine(_basePath, filePath);
        
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
        
        return Task.CompletedTask;
    }
    
    public string GetFileUrl(string filePath)
    {
        return $"/uploads/{filePath.Replace("\\", "/")}";
    }
}
