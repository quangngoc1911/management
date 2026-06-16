using ManagementSystem.Domain.Enums;

namespace ManagementSystem.Modules.Finance.Application.DTOs;

public class TransactionDto
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public string AccountName { get; set; } = string.Empty;
    public Guid? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public Guid? MemberId { get; set; }
    public string? MemberName { get; set; }
    public string Type { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "VND";
    public decimal? ExchangeRate { get; set; }
    public string? Description { get; set; }
    public string? Note { get; set; }
    public DateOnly TransactionDate { get; set; }
    public TransactionStatus Status { get; set; }
    public Guid? TransferToAccountId { get; set; }
    public string? TransferToAccountName { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
