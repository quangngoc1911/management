using System;
using System.Collections.Generic;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.Auth.Domain.Entities;

namespace ManagementSystem.Modules.Documents.Domain.Entities;

public class Comment : BaseEntity
{
    public Guid DocumentId { get; set; }
    public Document? Document { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }

    public string Content { get; set; } = string.Empty;
    public Guid? ParentId { get; set; }
    public Comment? Parent { get; set; }
    public ICollection<Comment> Replies { get; set; } = new List<Comment>();
}