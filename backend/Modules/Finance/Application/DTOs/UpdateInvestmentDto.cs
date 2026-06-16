namespace ManagementSystem.Modules.Finance.Application.DTOs;

public class UpdateInvestmentDto
{
    public Guid? AccountId { get; set; }
    public Guid? MemberId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Symbol { get; set; }
    public decimal? Quantity { get; set; }
    public decimal? PurchasePrice { get; set; }
    public decimal? CurrentPrice { get; set; }
    public bool IsActive { get; set; } = true;
}
