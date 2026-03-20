namespace MyAPI.Models;

using System.ComponentModel.DataAnnotations;

public class Categories
{
    public int CategoriesId { get; set; }
    [StringLength(256)]
    public string? CategoriesName { get; set; }
    [StringLength(256)]
    public string? Slug { get; set; }
    [StringLength(256)]
    public string? Description { get; set; }

    public string? Icon { get; set; }

    public string? CoverImageUrl { get; set; }

    public string? ParentId { get; set; }

    public string? SortOrder { get; set; }

    public int? IsActive { get; set; }
    public string? CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }
    // self FK
    public int? ParentsId { get; set; }
    public Categories Parent { get; set; }

    public List<Categories> Children { get; set; }

    public List<Documents> Documents { get; set; }
}