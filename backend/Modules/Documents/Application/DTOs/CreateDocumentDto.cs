using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ManagementSystem.Modules.Documents.Application.DTOs;

public class CreateDocumentDto
{
    [Required]
    [StringLength(500, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Slug { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Summary { get; set; }

    [Required]
    [StringLength(10)]
    public string ContentType { get; set; } = "text";

    public string? Content { get; set; }

    public Guid? FileId { get; set; }

    [StringLength(500)]
    public string? ThumbnailUrl { get; set; }

    [Required]
    public Guid CategoryId { get; set; }

    public Guid? MemberId { get; set; }

    public bool IsPublished { get; set; }
    public int SortOrder { get; set; }

    /// <summary>
    /// Optional file upload (sets FileId after upload)
    /// </summary>
    public IFormFile? File { get; set; }
}
