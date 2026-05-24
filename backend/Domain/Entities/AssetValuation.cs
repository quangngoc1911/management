using System;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.Auth.Domain.Entities;

namespace ManagementSystem.Modules.Assets.Domain.Entities;

public class AssetValuation : BaseEntity
{
    public Guid AssetId { get; set; }
    public Asset? Asset { get; set; }

    public DateOnly ValuationDate { get; set; }
    public decimal Value { get; set; }
    public string Currency { get; set; } = "VND";
    public string? ValuationMethod { get; set; }
    public string? Notes { get; set; }

    public Guid? CreatedByUserId { get; set; }
    public User? CreatedByUser { get; set; }
}
