namespace ManagementSystem.Modules.Finance.Application.DTOs;

public class AccountDto
{
    public Guid Id { get; set; }
    public Guid? MemberId { get; set; }
    public string? MemberName { get; set; }
    public string Name { get; set; } = string.Empty;
    public string AccountType { get; set; } = string.Empty;
    public string? AccountNumber { get; set; }
    public string? BankName { get; set; }
    public string Currency { get; set; } = "VND";
    public bool IsActive { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
