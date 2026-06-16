namespace ManagementSystem.Modules.Medical.Application.DTOs;

public class UpdateMedicalRecordDto
{
    public Guid MemberId { get; set; }
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

    /// <summary>Set by the controller from the authenticated user.</summary>
    public Guid? UpdatedByUserId { get; set; }
}
