namespace ManagementSystem.Modules.SystemAdmin.Application.DTOs;

public class BackupLogDto
{
    public Guid Id { get; set; }
    public string BackupType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? FilePath { get; set; }
    public long? FileSize { get; set; }
    public string? Checksum { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime? CreatedAt { get; set; }
}
