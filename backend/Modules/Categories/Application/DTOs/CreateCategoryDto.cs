using System.ComponentModel.DataAnnotations;

using ManagementSystem.Modules.Categories.Domain.Entities;

namespace ManagementSystem.Modules.Categories.Application.DTOs;

/// <summary>
/// Create new category request
/// </summary>
public class CreateCategoryDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Slug { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [MaxLength(100)]
    public string? Icon { get; set; }

    [MaxLength(500)]
    public string? CoverImageUrl { get; set; }

    public Guid? ParentId { get; set; }

    public int SortOrder { get; set; } = 0;

    public bool IsActive { get; set; } = true;
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }

}