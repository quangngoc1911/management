using System;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.Auth.Domain.Entities;

namespace ManagementSystem.Modules.Documents.Domain.Entities;

public class Favorite : BaseEntity
{
    public Guid DocumentId { get; set; }
    public Document? Document { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }
}