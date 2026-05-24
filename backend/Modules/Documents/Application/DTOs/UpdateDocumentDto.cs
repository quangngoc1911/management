using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ManagementSystem.Modules.Documents.Application.DTOs;

public class UpdateDocumentDto
{
    public Guid Id { get; set; }

    [StringLength(500, MinimumLength = 3)]
    public string? Title { get; set; }

    [StringLength(500)]
    public string? Slug { get; set; }

    [StringLength(2000)]
    public string? Summary { get; set; }

    [StringLength(10)]
    public string? ContentType { get; set; }

    public string? Content { get; set; }

    public Guid? FileId { get; set; }

    [StringLength(500)]
    public string? ThumbnailUrl { get; set; }

    public Guid? CategoryId { get; set; }
    public Guid? MemberId { get; set; }

    public bool? IsPublished { get; set; }
    public int? SortOrder { get; set; }

    public IFormFile? File { get; set; }

    /// <summary>
    /// Set to true to clear file attachment
    /// </summary>
    public bool RemoveFile { get; set; }
}
