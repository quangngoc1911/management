using System;

namespace ManagementSystem.Entities;

public class Favorite : BaseEntity
{
    public Guid DocumentId { get; set; }
    public Document? Document { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }
}