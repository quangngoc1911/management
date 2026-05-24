using System;
using System.Collections.Generic;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.Family.Domain.Entities;

namespace ManagementSystem.Modules.Finance.Domain.Entities;

public class Account : BaseEntity
{
    public Guid? MemberId { get; set; }
    public FamilyMember? Member { get; set; }

    public string Name { get; set; } = string.Empty;
    public string AccountType { get; set; } = string.Empty;
    public string? AccountNumber { get; set; }
    public string? BankName { get; set; }
    public string Currency { get; set; } = "VND";
    public new bool IsActive { get; set; } = true;

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<Transaction> IncomingTransfers { get; set; } = new List<Transaction>();
    public ICollection<Investment> Investments { get; set; } = new List<Investment>();
    public ICollection<RecurringTransaction> RecurringTransactions { get; set; } = new List<RecurringTransaction>();
}
