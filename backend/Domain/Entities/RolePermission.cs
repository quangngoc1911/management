using System;
using ManagementSystem.Domain.Entities;

namespace ManagementSystem.Modules.Auth.Domain.Entities;

public class RolePermission : BaseEntity
{
    public Guid RoleId { get; set; }
    public Role? Role { get; set; }

    public Guid PermissionId { get; set; }
    public Permission? Permission { get; set; }

    public Guid? GrantedBy { get; set; }
    public User? Granter { get; set; }

    public DateTime? RevokedAt { get; set; }
}
