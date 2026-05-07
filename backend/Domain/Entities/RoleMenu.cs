using System;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.Menus.Domain.Entities;

namespace ManagementSystem.Modules.Auth.Domain.Entities;

public class RoleMenu : BaseEntity
{
    public Guid RoleId { get; set; }
    public Role? Role { get; set; }

    public Guid MenuId { get; set; }
    public Menu? Menu { get; set; }
}