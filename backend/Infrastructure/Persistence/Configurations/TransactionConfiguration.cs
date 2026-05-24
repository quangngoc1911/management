using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Modules.Finance.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).HasColumnOrder(0);

        builder.HasIndex(t => t.AccountId);
        builder.Property(t => t.AccountId).IsRequired().HasColumnOrder(1);

        builder.Property(t => t.CategoryId).HasColumnOrder(2);
        builder.Property(t => t.MemberId).HasColumnOrder(3);

        builder.Property(t => t.Type)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnOrder(4);

        builder.Property(t => t.Amount).IsRequired().HasPrecision(15, 2).HasColumnOrder(5);

        builder.Property(t => t.Currency)
            .IsRequired()
            .HasMaxLength(3)
            .IsFixedLength()
            .HasColumnOrder(6);

        builder.Property(t => t.ExchangeRate).HasPrecision(12, 6).HasColumnOrder(7);

        builder.Property(t => t.Description).HasMaxLength(500).HasColumnOrder(8);
        builder.Property(t => t.Note).HasColumnType("text").HasColumnOrder(9);

        builder.HasIndex(t => t.TransactionDate);
        builder.Property(t => t.TransactionDate).IsRequired().HasColumnOrder(10);

        builder.Property(t => t.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasColumnOrder(11);

        builder.Property(t => t.CreatedBy).HasColumnOrder(12);
        builder.Property(t => t.TransferToAccountId).HasColumnOrder(13);
        builder.Property(t => t.RecurringTransactionId).HasColumnOrder(14);
        builder.Property(t => t.ReceiptFileId).HasColumnOrder(15);

        builder.Property(t => t.CreatedAt).IsRequired().HasColumnOrder(16);

        // Relationships
        builder.HasOne(t => t.Account)
            .WithMany(a => a.Transactions)
            .HasForeignKey(t => t.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Category)
            .WithMany()
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(t => t.Member)
            .WithMany()
            .HasForeignKey(t => t.MemberId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(t => t.Creator)
            .WithMany()
            .HasForeignKey(t => t.CreatedBy)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(t => t.TransferToAccount)
            .WithMany(a => a.IncomingTransfers)
            .HasForeignKey(t => t.TransferToAccountId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(t => t.RecurringTransaction)
            .WithMany(rt => rt.Transactions)
            .HasForeignKey(t => t.RecurringTransactionId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(t => t.ReceiptFile)
            .WithMany()
            .HasForeignKey(t => t.ReceiptFileId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
