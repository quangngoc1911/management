using System;
using ManagementSystem.Domain.Entities;
using System.Net;
namespace ManagementSystem.Modules.Auth.Domain.Entities;

public class LoginAttempt : BaseEntity
{
    public string Identifier { get; set; } = string.Empty;
    public Guid? UserId { get; set; }
    public User? User { get; set; }

    public IPAddress? IpAddress { get; set; }
    public bool Success { get; set; }
    public DateTime AttemptedAt { get; set; }
}
