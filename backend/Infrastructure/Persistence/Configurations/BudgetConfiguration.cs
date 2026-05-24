using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Modules.Finance.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).HasColumnOrder(0);

        builder.Property(b => b.CategoryId).HasColumnOrder(1);
        builder.Property(b => b.MemberId).HasColumnOrder(2);

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(255)
            .IsUnicode(true)
            .HasColumnOrder(3);

        builder.Property(b => b.Amount).IsRequired().HasPrecision(15, 2).HasColumnOrder(4);

        builder.Property(b => b.Currency)
            .IsRequired()
            .HasMaxLength(3)
            .IsFixedLength()
            .HasColumnOrder(5);

        builder.Property(b => b.PeriodType)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnOrder(6);

        builder.Property(b => b.StartDate).IsRequired().HasColumnOrder(7);
        builder.Property(b => b.EndDate).IsRequired().HasColumnOrder(8);

        builder.Property(b => b.AlertThreshold).HasPrecision(5, 2).HasColumnOrder(9);

        builder.Property(b => b.IsActive)
            .IsRequired()
            .HasDefaultValue(true)
            .HasColumnOrder(10);

        builder.Property(b => b.CreatedAt).IsRequired().HasColumnOrder(11);

        builder.HasOne(b => b.Category)
            .WithMany()
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Member)
            .WithMany()
            .HasForeignKey(b => b.MemberId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
