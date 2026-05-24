using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Modules.Finance.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class RecurringTransactionConfiguration : IEntityTypeConfiguration<RecurringTransaction>
{
    public void Configure(EntityTypeBuilder<RecurringTransaction> builder)
    {
        builder.HasKey(rt => rt.Id);
        builder.Property(rt => rt.Id).HasColumnOrder(0);

        builder.Property(rt => rt.AccountId).IsRequired().HasColumnOrder(1);
        builder.Property(rt => rt.CategoryId).HasColumnOrder(2);

        builder.Property(rt => rt.Name)
            .IsRequired()
            .HasMaxLength(255)
            .IsUnicode(true)
            .HasColumnOrder(3);

        builder.Property(rt => rt.Type)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnOrder(4);

        builder.Property(rt => rt.Amount).IsRequired().HasPrecision(15, 2).HasColumnOrder(5);

        builder.Property(rt => rt.Frequency)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnOrder(6);

        builder.Property(rt => rt.StartDate).IsRequired().HasColumnOrder(7);
        builder.Property(rt => rt.EndDate).HasColumnOrder(8);

        builder.HasIndex(rt => rt.NextDueDate);
        builder.Property(rt => rt.NextDueDate).HasColumnOrder(9);

        builder.Property(rt => rt.IsActive)
            .IsRequired()
            .HasDefaultValue(true)
            .HasColumnOrder(10);

        builder.Property(rt => rt.CreatedAt).IsRequired().HasColumnOrder(11);

        builder.HasOne(rt => rt.Account)
            .WithMany(a => a.RecurringTransactions)
            .HasForeignKey(rt => rt.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(rt => rt.Category)
            .WithMany()
            .HasForeignKey(rt => rt.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
