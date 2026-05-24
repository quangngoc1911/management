using System;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Domain.Enums;
using ManagementSystem.Modules.Family.Domain.Entities;

namespace ManagementSystem.Modules.Education.Domain.Entities;

public class StudySchedule : BaseEntity
{
    public Guid MemberId { get; set; }
    public FamilyMember? Member { get; set; }

    public Guid? EducationRecordId { get; set; }
    public EducationRecord? EducationRecord { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Subject { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? RecurrenceRule { get; set; }
    public string? Location { get; set; }
    public bool? IsOnline { get; set; }
    public string? MeetingUrl { get; set; }
    public string? TeacherName { get; set; }
    public new StudyScheduleStatus Status { get; set; } = StudyScheduleStatus.Scheduled;
}
