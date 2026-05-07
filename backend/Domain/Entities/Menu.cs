using System;
using System.Collections.Generic;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.Auth.Domain.Entities;

namespace ManagementSystem.Modules.Menus.Domain.Entities;

public class Menu : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Path { get; set; }
    public string? Icon { get; set; }
    public Guid? ParentId { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public Menu? Parent { get; set; }
    public ICollection<Menu> Children { get; set; } = new List<Menu>();
    public ICollection<RoleMenu> RoleMenus { get; set; } = new List<RoleMenu>();
}