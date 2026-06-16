namespace ManagementSystem.Modules.SystemAdmin.Application.DTOs;

public class SystemConfigDto
{
    public Guid Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = "{}";
    public string? Description { get; set; }
    public bool IsEncrypted { get; set; }
    public bool IsPublic { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
