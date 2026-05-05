using System;
using System.Collections.Generic;

namespace ManagementSystem.Entities;

public class Tag : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Color { get; set; }

    // Navigation properties
    public ICollection<DocumentTag> DocumentTags { get; set; } = new List<DocumentTag>();
}