using System;
using ManagementSystem.Modules.Auth.Domain.Entities;

namespace ManagementSystem.Domain.Entities;

public class Notification : BaseEntity
{
    public Guid UserId { get; set; }
    public User? User { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Body { get; set; }

    public string Type { get; set; } = string.Empty;
    public string Channel { get; set; } = string.Empty;

    public string? EntityType { get; set; }
    public Guid? EntityId { get; set; }

    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
    public DateTime? SentAt { get; set; }
}
