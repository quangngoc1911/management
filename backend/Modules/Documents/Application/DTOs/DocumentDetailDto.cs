namespace ManagementSystem.Modules.Documents.Application.DTOs;

/// <summary>
/// Document detail for single page view
/// </summary>
public class DocumentDetailDto : DocumentListDto
{
    public string? Content { get; set; }
    public string? FileUrl { get; set; }
    public string? OriginalFileName { get; set; }
    public long? FileSizeBytes { get; set; }
    public bool IsPublished { get; set; }
    public int Version { get; set; }
}
