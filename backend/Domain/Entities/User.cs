using System;
using System.Collections.Generic;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.Documents.Domain.Entities;

namespace ManagementSystem.Modules.Auth.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string? UserName { get; set; }
    public string? NormalizedUserName { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? NormalizedPhone { get; set; }
    public string? AvatarUrl { get; set; }
    public DateTime? EmailVerifiedAt { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public string? TwoFactorSecret { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public short FailedLoginCount { get; set; }
    public DateTime? LockedUntil { get; set; }
    public DateTime? PasswordChangedAt { get; set; }

    // Auth navigations
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<UserSession> Sessions { get; set; } = new List<UserSession>();
    public ICollection<PasswordHistory> PasswordHistories { get; set; } = new List<PasswordHistory>();
    public ICollection<LoginAttempt> LoginAttempts { get; set; } = new List<LoginAttempt>();
    public ICollection<SecurityLog> SecurityLogs { get; set; } = new List<SecurityLog>();

    // Documents navigations
    public ICollection<Document> CreatedDocuments { get; set; } = new List<Document>();
    public ICollection<DocumentVersion> EditedVersions { get; set; } = new List<DocumentVersion>();

    // System navigations
    public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
}
