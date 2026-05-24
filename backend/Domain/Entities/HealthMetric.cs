using System;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.Family.Domain.Entities;

namespace ManagementSystem.Modules.Medical.Domain.Entities;

public class HealthMetric : BaseEntity
{
    public Guid MemberId { get; set; }
    public FamilyMember? Member { get; set; }

    public string MetricType { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public string Unit { get; set; } = string.Empty;
    public DateTime MeasuredAt { get; set; }
    public string? Notes { get; set; }
}
