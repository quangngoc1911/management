using System;
using System.Collections.Generic;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.Documents.Domain.Entities;
using ManagementSystem.Modules.Auth.Domain.Entities;

namespace ManagementSystem.Modules.Auth.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "Viewer"; // Temporary for compatibility

    public string? AvatarUrl { get; set; }
    public string? Department { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? LastLoginAt { get; set; }

    // Navigation properties
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<Document> CreatedDocuments { get; set; } = new List<Document>();
    public ICollection<DocumentVersion> EditedVersions { get; set; } = new List<DocumentVersion>();
    public ICollection<Attachment> UploadedFiles { get; set; } = new List<Attachment>();
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<DocumentShare> SharedDocuments { get; set; } = new List<DocumentShare>();
    public ICollection<DocumentShare> ReceivedShares { get; set; } = new List<DocumentShare>();
    public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
}