using System;
using System.Collections.Generic;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Domain.Enums;
using ManagementSystem.Modules.Auth.Domain.Entities;
using ManagementSystem.Modules.Categories.Domain.Entities;
using ManagementSystem.Modules.Family.Domain.Entities;

namespace ManagementSystem.Modules.Assets.Domain.Entities;

public class Asset : BaseEntity
{
    public Guid? MemberId { get; set; }
    public FamilyMember? Member { get; set; }

    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string AssetType { get; set; } = string.Empty;
    public decimal? PurchasePrice { get; set; }
    public DateOnly? PurchaseDate { get; set; }
    public string? Currency { get; set; }
    public string? Location { get; set; }
    public string? SerialNumber { get; set; }

    public new AssetStatus Status { get; set; } = AssetStatus.Active;

    public bool? IsInsured { get; set; }
    public string? InsuranceInfo { get; set; }
    public string? Metadata { get; set; }

    public Guid? CreatedByUserId { get; set; }
    public User? CreatedByUser { get; set; }

    public ICollection<AssetValuation> Valuations { get; set; } = new List<AssetValuation>();
}
