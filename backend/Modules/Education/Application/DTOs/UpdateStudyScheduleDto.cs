using ManagementSystem.Domain.Enums;

namespace ManagementSystem.Modules.Education.Application.DTOs;

public class UpdateStudyScheduleDto
{
    public Guid MemberId { get; set; }
    public Guid? EducationRecordId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Subject { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? RecurrenceRule { get; set; }
    public string? Location { get; set; }
    public bool? IsOnline { get; set; }
    public string? MeetingUrl { get; set; }
    public string? TeacherName { get; set; }
    public StudyScheduleStatus Status { get; set; } = StudyScheduleStatus.Scheduled;
}
