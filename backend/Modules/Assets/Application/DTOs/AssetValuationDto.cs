namespace ManagementSystem.Modules.Assets.Application.DTOs;

public class AssetValuationDto
{
    public Guid Id { get; set; }
    public Guid AssetId { get; set; }
    public string AssetName { get; set; } = string.Empty;
    public DateOnly ValuationDate { get; set; }
    public decimal Value { get; set; }
    public string Currency { get; set; } = "VND";
    public string? ValuationMethod { get; set; }
    public string? Notes { get; set; }
    public Guid? CreatedByUserId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
