using System;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.Auth.Domain.Entities;

namespace ManagementSystem.Modules.Utility.Domain.Entities;

public class ViewHistory : BaseEntity
{
    public Guid UserId { get; set; }
    public User? User { get; set; }

    public string EntityType { get; set; } = string.Empty;
    public Guid EntityId { get; set; }

    public DateTime ViewedAt { get; set; }
    public int? DurationSeconds { get; set; }
}
