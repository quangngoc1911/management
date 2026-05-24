using System;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.Categories.Domain.Entities;
using ManagementSystem.Modules.Family.Domain.Entities;

namespace ManagementSystem.Modules.Finance.Domain.Entities;

public class Budget : BaseEntity
{
    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }

    public Guid? MemberId { get; set; }
    public FamilyMember? Member { get; set; }

    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "VND";
    public string PeriodType { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal? AlertThreshold { get; set; }
    public new bool IsActive { get; set; } = true;
}
