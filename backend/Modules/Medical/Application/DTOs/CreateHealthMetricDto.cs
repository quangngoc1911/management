namespace ManagementSystem.Modules.Medical.Application.DTOs;

public class CreateHealthMetricDto
{
    public Guid MemberId { get; set; }
    public string MetricType { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public string Unit { get; set; } = string.Empty;
    public DateTime MeasuredAt { get; set; }
    public string? Notes { get; set; }
}
