using System;
using System.Collections.Generic;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.Auth.Domain.Entities;
using ManagementSystem.Modules.Family.Domain.Entities;

namespace ManagementSystem.Modules.Medical.Domain.Entities;

public class MedicalRecord : BaseEntity
{
    public Guid MemberId { get; set; }
    public FamilyMember? Member { get; set; }

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

    public Guid? CreatedByUserId { get; set; }
    public User? CreatedByUser { get; set; }

    public Guid? UpdatedByUserId { get; set; }
    public User? UpdatedByUser { get; set; }

    public Guid? DeletedByUserId { get; set; }
    public User? DeletedByUser { get; set; }

    public ICollection<Medication> Medications { get; set; } = new List<Medication>();
}
