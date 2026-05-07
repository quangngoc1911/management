using System.ComponentModel.DataAnnotations;

namespace ManagementSystem.Modules.Documents.Application.DTOs;

/// <summary>
/// DTO for updating an existing document
/// </summary>
public class UpdateDocumentDto
{
    public Guid Id { get; set; }

    [StringLength(500, MinimumLength = 3)]
    public string? Title { get; set; }

    [StringLength(2000)]
    public string? Description { get; set; }

    [StringLength(100)]
    public string? DocumentNumber { get; set; }

    public Guid? CategoryId { get; set; }

    public DateTime? IssueDate { get; set; }

    public DateTime? ExpiryDate { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    /// <summary>
    /// File to upload (optional) - replaces existing file
    /// </summary>
    public IFormFile? File { get; set; }

    /// <summary>
    /// Set to true to remove the existing file
    /// </summary>
    public bool RemoveFile { get; set; }

    /// <summary>
    /// Additional custom fields to update
    /// </summary>
    public List<UpdateDocumentFieldDto> Fields { get; set; } = new();
}
