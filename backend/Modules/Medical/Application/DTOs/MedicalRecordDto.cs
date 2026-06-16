namespace ManagementSystem.Modules.Medical.Application.DTOs;

public class MedicalRecordDto
{
    public Guid Id { get; set; }
    public Guid MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public string RecordType { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public string? DoctorName { get; set; }
    public string? HospitalName { get; set; }
    public DateOnly RecordDate { get; set; }
    public DateOnly? FollowUpDate { get; set; }
    public bool IsPrivate { get; set; }
    public string? Notes { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
