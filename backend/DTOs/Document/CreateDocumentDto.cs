using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ManagementSystem.DTOs.Document;

/// <summary>
/// DTO for creating a new document
/// </summary>
public class CreateDocumentDto
{
    [Required]
    [StringLength(500, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Description { get; set; }

    [StringLength(100)]
    public string? DocumentNumber { get; set; }

    [Required]
    public Guid CategoryId { get; set; }

    public DateTime? IssueDate { get; set; }

    public DateTime? ExpiryDate { get; set; }

    [StringLength(50)]
    public string? Status { get; set; } = "Draft";

    /// <summary>
    /// File to upload (optional)
    /// </summary>
    public IFormFile? File { get; set; }

    /// <summary>
    /// Additional custom fields
    /// </summary>
    public List<CreateDocumentFieldDto> Fields { get; set; } = new();
}
