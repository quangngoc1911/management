namespace ManagementSystem.Modules.Assets.Application.DTOs;

public class UpdateAssetValuationDto
{
    public DateOnly ValuationDate { get; set; }
    public decimal Value { get; set; }
    public string Currency { get; set; } = "VND";
    public string? ValuationMethod { get; set; }
    public string? Notes { get; set; }
}
