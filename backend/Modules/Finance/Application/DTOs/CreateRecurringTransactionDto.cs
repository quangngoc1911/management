namespace ManagementSystem.Modules.Finance.Application.DTOs;

public class CreateRecurringTransactionDto
{
    public Guid AccountId { get; set; }
    public Guid? CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Frequency { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public DateOnly? NextDueDate { get; set; }
    public bool IsActive { get; set; } = true;
}
