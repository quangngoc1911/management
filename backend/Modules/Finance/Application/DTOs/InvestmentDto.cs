namespace ManagementSystem.Modules.Finance.Application.DTOs;

public class InvestmentDto
{
    public Guid Id { get; set; }
    public Guid? AccountId { get; set; }
    public string? AccountName { get; set; }
    public Guid? MemberId { get; set; }
    public string? MemberName { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Symbol { get; set; }
    public decimal? Quantity { get; set; }
    public decimal? PurchasePrice { get; set; }
    public decimal? CurrentPrice { get; set; }
    public bool IsActive { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
