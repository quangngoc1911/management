using System;
using ManagementSystem.Domain.Enums;
using ManagementSystem.Modules.Auth.Domain.Entities;
using System.Net;
namespace ManagementSystem.Domain.Entities;

public class AuditLog : BaseEntity
{
    public Guid? UserId { get; set; }
    public User? User { get; set; }

    public AuditAction Action { get; set; }
    public string EntityType { get; set; } = string.Empty;
    public Guid? EntityId { get; set; }

    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public IPAddress? IpAddress { get; set; }
}
