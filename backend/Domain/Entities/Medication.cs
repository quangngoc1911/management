using System;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.Family.Domain.Entities;

namespace ManagementSystem.Modules.Medical.Domain.Entities;

public class Medication : BaseEntity
{
    public Guid MemberId { get; set; }
    public FamilyMember? Member { get; set; }

    public Guid? MedicalRecordId { get; set; }
    public MedicalRecord? MedicalRecord { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Dosage { get; set; }
    public string? Frequency { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? ReminderTimes { get; set; }
    public new bool IsActive { get; set; } = true;
    public string? Notes { get; set; }
}
