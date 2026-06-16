namespace ManagementSystem.Modules.Assets.Application.DTOs;

public class CreateAssetValuationDto
{
    public Guid AssetId { get; set; }
    public DateOnly ValuationDate { get; set; }
    public decimal Value { get; set; }
    public string Currency { get; set; } = "VND";
    public string? ValuationMethod { get; set; }
    public string? Notes { get; set; }

    /// <summary>Set by the controller from the authenticated user.</summary>
    public Guid? CreatedByUserId { get; set; }
}
