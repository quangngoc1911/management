using System;

namespace ManagementSystem.Domain.Entities;

public class BackupLog : BaseEntity
{
    public string BackupType { get; set; } = string.Empty;
    public new string Status { get; set; } = "pending";

    public string? FilePath { get; set; }
    public long? FileSize { get; set; }
    public string? Checksum { get; set; }

    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? ErrorMessage { get; set; }
}
