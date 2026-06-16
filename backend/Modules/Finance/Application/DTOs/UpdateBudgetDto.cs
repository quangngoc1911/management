namespace ManagementSystem.Modules.Finance.Application.DTOs;

public class UpdateBudgetDto
{
    public Guid? CategoryId { get; set; }
    public Guid? MemberId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "VND";
    public string PeriodType { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal? AlertThreshold { get; set; }
    public bool IsActive { get; set; } = true;
}
