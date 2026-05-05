using System;
using System.Collections.Generic;

namespace ManagementSystem.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsSystemRole { get; set; } = false;

    // Navigation properties
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<RoleMenu> RoleMenus { get; set; } = new List<RoleMenu>();
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}