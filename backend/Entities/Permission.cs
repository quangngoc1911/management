using System;
using System.Collections.Generic;

namespace ManagementSystem.Entities;

public class Permission : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Resource { get; set; } = string.Empty; // e.g., "Document", "User"
    public string Action { get; set; } = string.Empty; // e.g., "Create", "Read", "Update", "Delete"

    // Navigation properties
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}