using System;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Domain.Enums;
using System.Net;
namespace ManagementSystem.Modules.Auth.Domain.Entities;

public class SecurityLog : BaseEntity
{
    public Guid? UserId { get; set; }
    public User? User { get; set; }

    public string EventType { get; set; } = string.Empty;
    public IPAddress? IpAddress { get; set; }
    public RiskLevel? RiskLevel { get; set; }
    public string? UserAgent { get; set; }
    public string? Browser { get; set; }
    public string? Os { get; set; }
    public DeviceType? DeviceType { get; set; }
    public SecurityEventStatus? EventStatus { get; set; }
}
