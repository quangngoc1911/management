using ManagementSystem.Domain.Enums;

namespace ManagementSystem.Modules.Family.Application.DTOs;

/// <summary>
/// Request payload to create or update a family member's profile (full replace semantics).
/// </summary>
public class UpsertMemberProfileDto
{
    public string? NationalId { get; set; }
    public string? PassportNo { get; set; }
    public string? Nationality { get; set; }
    public string? Ethnicity { get; set; }
    public string? Religion { get; set; }
    public string? BloodType { get; set; }
    public MaritalStatus? MaritalStatus { get; set; }
    public EducationLevel? EducationLevel { get; set; }
    public string? Occupation { get; set; }
    public string? BirthPlace { get; set; }
    public string? CurrentAddress { get; set; }
    public string? PermanentAddress { get; set; }
    public decimal? HeightCm { get; set; }
    public decimal? WeightKg { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? Bio { get; set; }
}
