using System;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.Family.Domain.Entities;

namespace ManagementSystem.Modules.Finance.Domain.Entities;

public class Investment : BaseEntity
{
    public Guid? AccountId { get; set; }
    public Account? Account { get; set; }

    public Guid? MemberId { get; set; }
    public FamilyMember? Member { get; set; }

    public string Type { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Symbol { get; set; }
    public decimal? Quantity { get; set; }
    public decimal? PurchasePrice { get; set; }
    public decimal? CurrentPrice { get; set; }
    public new bool IsActive { get; set; } = true;
}
