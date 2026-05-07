using System;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.Auth.Domain.Entities;

namespace ManagementSystem.Modules.Documents.Domain.Entities;

public class DocumentShare : BaseEntity
{
    public Guid DocumentId { get; set; }
    public Document? Document { get; set; }

    public Guid SharedWithUserId { get; set; }
    public User? SharedWithUser { get; set; }

    public Guid SharedByUserId { get; set; }
    public User? SharedByUser { get; set; }

    public string PermissionLevel { get; set; } = "Read"; // Read, Write, Admin
    public DateTime? ExpiresAt { get; set; }
}