using System;
using ManagementSystem.Domain.Entities;

namespace ManagementSystem.Modules.Auth.Domain.Entities;

public class UserSession : BaseEntity
{
    public Guid UserId { get; set; }
    public User? User { get; set; }

    public string SessionToken { get; set; } = string.Empty;
    public DateTime LastActiveAt { get; set; }
    public DateTime ExpiresAt { get; set; }
}
