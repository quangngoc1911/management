using System;
using System.Collections.Generic;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.Documents.Domain.Entities;

namespace ManagementSystem.Modules.Categories.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public string? CoverImageUrl { get; set; }
    public Guid? ParentId { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public int DocumentCount { get; set; }

    public Category? Parent { get; set; }

    public ICollection<Category> Children { get; set; } = new List<Category>();
    public ICollection<Document> Documents { get; set; } = new List<Document>();
}