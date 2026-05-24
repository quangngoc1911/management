using System;
using ManagementSystem.Domain.Entities;

namespace ManagementSystem.Modules.Auth.Domain.Entities;

public class PasswordHistory : BaseEntity
{
    public Guid UserId { get; set; }
    public User? User { get; set; }

    public string PasswordHash { get; set; } = string.Empty;
    public DateTime ChangedAt { get; set; }
}
