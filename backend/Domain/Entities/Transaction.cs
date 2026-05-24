using System;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Domain.Enums;
using ManagementSystem.Modules.Auth.Domain.Entities;
using ManagementSystem.Modules.Categories.Domain.Entities;
using ManagementSystem.Modules.Documents.Domain.Entities;
using ManagementSystem.Modules.Family.Domain.Entities;

namespace ManagementSystem.Modules.Finance.Domain.Entities;

public class Transaction : BaseEntity
{
    public Guid AccountId { get; set; }
    public Account? Account { get; set; }

    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }

    public Guid? MemberId { get; set; }
    public FamilyMember? Member { get; set; }

    public string Type { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "VND";
    public decimal? ExchangeRate { get; set; }

    public string? Description { get; set; }
    public string? Note { get; set; }
    public DateOnly TransactionDate { get; set; }

    public new TransactionStatus Status { get; set; } = TransactionStatus.Completed;

    public Guid? CreatedBy { get; set; }
    public User? Creator { get; set; }

    public Guid? TransferToAccountId { get; set; }
    public Account? TransferToAccount { get; set; }

    public Guid? RecurringTransactionId { get; set; }
    public RecurringTransaction? RecurringTransaction { get; set; }

    public Guid? ReceiptFileId { get; set; }
    public DocumentFile? ReceiptFile { get; set; }
}
