using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Modules.Utility.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class ReminderConfiguration : IEntityTypeConfiguration<Reminder>
{
    public void Configure(EntityTypeBuilder<Reminder> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnOrder(0);

        builder.HasIndex(r => r.UserId);
        builder.Property(r => r.UserId).IsRequired().HasColumnOrder(1);

        builder.Property(r => r.MemberId).HasColumnOrder(2);

        builder.Property(r => r.Title)
            .IsRequired()
            .HasMaxLength(500)
            .IsUnicode(true)
            .HasColumnOrder(3);

        builder.Property(r => r.Description).HasColumnType("text").HasColumnOrder(4);

        builder.HasIndex(r => r.RemindAt);
        builder.Property(r => r.RemindAt).IsRequired().HasColumnOrder(5);

        builder.Property(r => r.RecurrenceRule).HasColumnType("text").HasColumnOrder(6);

        builder.Property(r => r.EntityType).HasMaxLength(100).HasColumnOrder(7);
        builder.Property(r => r.EntityId).HasColumnOrder(8);

        builder.Property(r => r.Status).IsRequired().HasConversion<int>().HasColumnOrder(9);

        builder.Property(r => r.SnoozedUntil).HasColumnOrder(10);

        builder.Property(r => r.CreatedAt).IsRequired().HasColumnOrder(11);

        builder.HasOne(r => r.User).WithMany().HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(r => r.Member).WithMany().HasForeignKey(r => r.MemberId).OnDelete(DeleteBehavior.SetNull);
    }
}
