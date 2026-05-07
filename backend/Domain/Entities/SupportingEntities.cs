using System;
using System.Collections.Generic;
using ManagementSystem.Modules.Auth.Domain.Entities;
using ManagementSystem.Modules.Documents.Domain.Entities;

namespace ManagementSystem.Domain.Entities;

public class DocumentFile : BaseEntity
{
    public string OriginalName { get; set; } = string.Empty;
    public string StoredName { get; set; } = string.Empty;
    public string StoragePath { get; set; } = string.Empty;
    public string PublicUrl { get; set; } = string.Empty;
    public string FileType { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;
    public long FileSize { get; set; }
    public string StorageProvider { get; set; } = "Local"; // Local, Azure, S3

    public Guid UploadedByUserId { get; set; }
    public User? UploadedByUser { get; set; }

    public ICollection<Document> Documents { get; set; } = new List<Document>();
}

public class Bookmark : BaseEntity
{
    public Guid UserId { get; set; }
    public User? User { get; set; }

    public Guid DocumentId { get; set; }
    public Document? Document { get; set; }

    public string? Note { get; set; }
}

public class ViewHistory : BaseEntity
{
    public Guid UserId { get; set; }
    public User? User { get; set; }

    public Guid DocumentId { get; set; }
    public Document? Document { get; set; }

    public DateTime ViewedAt { get; set; } = DateTime.UtcNow;
    public int Duration { get; set; } // in seconds
}

public class RefreshToken : BaseEntity
{
    public Guid UserId { get; set; }
    public User? User { get; set; }

    public string Token { get; set; } = string.Empty;
    public string TokenHash { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public string? CreatedByIp { get; set; }
    public string? DeviceInfo { get; set; }
}

public class SystemConfig : BaseEntity
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Group { get; set; }
}

public class DocumentComment : BaseEntity
{
    public Guid DocumentId { get; set; }
    public Document? Document { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }

    public string Content { get; set; } = string.Empty;
    public Guid? ParentId { get; set; }
}