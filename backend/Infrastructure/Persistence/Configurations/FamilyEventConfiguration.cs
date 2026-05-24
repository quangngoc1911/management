using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Modules.Events.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class FamilyEventConfiguration : IEntityTypeConfiguration<FamilyEvent>
{
    public void Configure(EntityTypeBuilder<FamilyEvent> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnOrder(0);

        builder.Property(e => e.Title).IsRequired().HasMaxLength(500).IsUnicode(true).HasColumnOrder(1);
        builder.Property(e => e.Description).HasColumnType("text").HasColumnOrder(2);

        builder.HasIndex(e => e.EventType);
        builder.Property(e => e.EventType).HasMaxLength(100).HasColumnOrder(3);

        builder.HasIndex(e => e.StartAt);
        builder.Property(e => e.StartAt).IsRequired().HasColumnOrder(4);
        builder.Property(e => e.EndAt).HasColumnOrder(5);

        builder.Property(e => e.AllDay).IsRequired().HasDefaultValue(false).HasColumnOrder(6);
        builder.Property(e => e.Location).HasColumnType("text").HasColumnOrder(7);
        builder.Property(e => e.RecurrenceRule).HasColumnType("text").HasColumnOrder(8);

        builder.Property(e => e.Status).IsRequired().HasConversion<int>().HasColumnOrder(9);

        builder.Property(e => e.CoverFileId).HasColumnOrder(10);
        builder.Property(e => e.CreatedByUserId).IsRequired().HasColumnName("created_by").HasColumnOrder(11);

        builder.Property(e => e.CreatedAt).IsRequired().HasColumnOrder(12);
        builder.Property(e => e.UpdatedAt).IsRequired().HasColumnOrder(13);
        builder.Property(e => e.DeletedAt).HasColumnOrder(14);
        builder.Property(e => e.IsDeleted).IsRequired().HasDefaultValue(false).HasColumnOrder(15);

        builder.HasOne(e => e.CoverFile).WithMany().HasForeignKey(e => e.CoverFileId).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(e => e.CreatedByUser).WithMany().HasForeignKey(e => e.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
    }
}
