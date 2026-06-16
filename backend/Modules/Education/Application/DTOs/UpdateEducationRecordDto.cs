using ManagementSystem.Domain.Enums;

namespace ManagementSystem.Modules.Education.Application.DTOs;

public class UpdateEducationRecordDto
{
    public Guid MemberId { get; set; }
    public string InstitutionName { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string? Major { get; set; }
    public string? Degree { get; set; }
    public string? StudentId { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public decimal? Gpa { get; set; }
    public EducationStatus Status { get; set; } = EducationStatus.Enrolled;
    public string? Achievements { get; set; }
    public string? Notes { get; set; }
}
