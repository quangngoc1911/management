using ManagementSystem.Domain.Enums;

namespace ManagementSystem.Modules.Finance.Application.DTOs;

public class UpdateTransactionDto
{
    public Guid AccountId { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? MemberId { get; set; }
    public string Type { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "VND";
    public decimal? ExchangeRate { get; set; }
    public string? Description { get; set; }
    public string? Note { get; set; }
    public DateOnly TransactionDate { get; set; }
    public TransactionStatus Status { get; set; } = TransactionStatus.Completed;
    public Guid? TransferToAccountId { get; set; }
}
