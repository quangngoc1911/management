using ManagementSystem.Domain.Enums;

namespace ManagementSystem.Modules.Assets.Application.DTOs;

public class UpdateAssetDto
{
    public Guid? MemberId { get; set; }
    public Guid? CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string AssetType { get; set; } = string.Empty;
    public decimal? PurchasePrice { get; set; }
    public DateOnly? PurchaseDate { get; set; }
    public string? Currency { get; set; }
    public string? Location { get; set; }
    public string? SerialNumber { get; set; }
    public AssetStatus Status { get; set; } = AssetStatus.Active;
    public bool? IsInsured { get; set; }
    public string? InsuranceInfo { get; set; }
}
