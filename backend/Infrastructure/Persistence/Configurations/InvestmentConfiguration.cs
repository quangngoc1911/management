using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Modules.Finance.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class InvestmentConfiguration : IEntityTypeConfiguration<Investment>
{
    public void Configure(EntityTypeBuilder<Investment> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).HasColumnOrder(0);

        builder.Property(i => i.AccountId).HasColumnOrder(1);
        builder.Property(i => i.MemberId).HasColumnOrder(2);

        builder.Property(i => i.Type)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnOrder(3);

        builder.Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(255)
            .IsUnicode(true)
            .HasColumnOrder(4);

        builder.Property(i => i.Symbol)
            .HasMaxLength(20)
            .HasColumnOrder(5);

        builder.Property(i => i.Quantity).HasPrecision(18, 6).HasColumnOrder(6);
        builder.Property(i => i.PurchasePrice).HasPrecision(15, 2).HasColumnOrder(7);
        builder.Property(i => i.CurrentPrice).HasPrecision(15, 2).HasColumnOrder(8);

        builder.Property(i => i.IsActive)
            .IsRequired()
            .HasDefaultValue(true)
            .HasColumnOrder(9);

        builder.Property(i => i.CreatedAt).IsRequired().HasColumnOrder(10);

        builder.HasOne(i => i.Account)
            .WithMany(a => a.Investments)
            .HasForeignKey(i => i.AccountId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(i => i.Member)
            .WithMany()
            .HasForeignKey(i => i.MemberId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
