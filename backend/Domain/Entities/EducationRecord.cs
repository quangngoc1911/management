using System;
using System.Collections.Generic;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Domain.Enums;
using ManagementSystem.Modules.Family.Domain.Entities;

namespace ManagementSystem.Modules.Education.Domain.Entities;

public class EducationRecord : BaseEntity
{
    public Guid MemberId { get; set; }
    public FamilyMember? Member { get; set; }

    public string InstitutionName { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string? Major { get; set; }
    public string? Degree { get; set; }
    public string? StudentId { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public decimal? Gpa { get; set; }
    public new EducationStatus Status { get; set; } = EducationStatus.Enrolled;
    public string? Achievements { get; set; }
    public string? Notes { get; set; }

    public ICollection<StudySchedule> StudySchedules { get; set; } = new List<StudySchedule>();
}
