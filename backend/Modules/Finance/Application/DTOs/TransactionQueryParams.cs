using ManagementSystem.Domain.Enums;

namespace ManagementSystem.Modules.Finance.Application.DTOs;

public class TransactionQueryParams
{
    public string? Search { get; set; }
    public Guid? AccountId { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? MemberId { get; set; }
    public string? Type { get; set; }
    public TransactionStatus? Status { get; set; }
    public DateOnly? FromDate { get; set; }
    public DateOnly? ToDate { get; set; }
    public string SortBy { get; set; } = "transactiondate";
    public bool IsDescending { get; set; } = true;
    public int Page { get; set; } = 1;

    private int _pageSize = 20;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 100 ? 100 : value < 1 ? 1 : value;
    }
}
