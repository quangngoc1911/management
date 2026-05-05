using System;
using System.Collections.Generic;

using ManagementSystem.Entities;

namespace ManagementSystem.DTOs.Menu;

public class MenuDto 
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Path { get; set; }
    public string? Icon { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public Guid? ParentId { get; set; }
    public List<MenuDto> Children { get; set; } = new();
}
