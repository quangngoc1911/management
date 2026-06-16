using ManagementSystem.Domain.Enums;

namespace ManagementSystem.Modules.Assets.Application.DTOs;

public class AssetDto
{
    public Guid Id { get; set; }
    public Guid? MemberId { get; set; }
    public string? MemberName { get; set; }
    public Guid? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string AssetType { get; set; } = string.Empty;
    public decimal? PurchasePrice { get; set; }
    public DateOnly? PurchaseDate { get; set; }
    public string? Currency { get; set; }
    public string? Location { get; set; }
    public string? SerialNumber { get; set; }
    public AssetStatus Status { get; set; }
    public bool? IsInsured { get; set; }
    public string? InsuranceInfo { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
