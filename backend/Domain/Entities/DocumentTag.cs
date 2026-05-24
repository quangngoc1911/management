using System;
using ManagementSystem.Domain.Entities;

namespace ManagementSystem.Modules.Documents.Domain.Entities;

public class DocumentTag : BaseEntity
{
    public Guid DocumentId { get; set; }
    public Document? Document { get; set; }

    public Guid TagId { get; set; }
    public Tag? Tag { get; set; }

    public DateTime AddedAt { get; set; }
}
