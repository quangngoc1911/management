using System;

using ManagementSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagementSystem.Domain.Entities;

public abstract class BaseEntity
{

    public Guid Id { get; set; } = Guid.NewGuid();

    [NotMapped]
    public EntityStatus? Status { get; set; } = EntityStatus.Active;
    [NotMapped]
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    [NotMapped]
    public DateTime? UpdatedAt { get; set; }
    [NotMapped]
    public DateTime? DeletedAt { get; set; }

    [NotMapped]
    public Guid? CreatedBy { get; set; }
    [NotMapped]
    public Guid? UpdatedBy { get; set; }
    [NotMapped]
    public Guid? DeletedBy { get; set; }

    [NotMapped]
    public bool IsDeleted { get; set; }
    [NotMapped]
    public bool IsActive { get; set; }

}
