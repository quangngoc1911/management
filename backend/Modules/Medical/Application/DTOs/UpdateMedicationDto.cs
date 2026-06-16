namespace ManagementSystem.Modules.Medical.Application.DTOs;

public class UpdateMedicationDto
{
    public Guid MemberId { get; set; }
    public Guid? MedicalRecordId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Dosage { get; set; }
    public string? Frequency { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? ReminderTimes { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Notes { get; set; }
}
