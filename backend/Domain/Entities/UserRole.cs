using System;
using ManagementSystem.Domain.Entities;

namespace ManagementSystem.Modules.Auth.Domain.Entities;

public class UserRole : BaseEntity
{
    public Guid UserId { get; set; }
    public User? User { get; set; }

    public Guid RoleId { get; set; }
    public Role? Role { get; set; }

    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    public Guid? AssignedBy { get; set; }
}