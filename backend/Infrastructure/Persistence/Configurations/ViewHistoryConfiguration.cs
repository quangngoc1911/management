using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Modules.Utility.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class ViewHistoryConfiguration : IEntityTypeConfiguration<ViewHistory>
{
    public void Configure(EntityTypeBuilder<ViewHistory> builder)
    {
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id).HasColumnOrder(0);

        builder.HasIndex(v => v.UserId);
        builder.Property(v => v.UserId).IsRequired().HasColumnOrder(1);

        builder.HasIndex(v => v.EntityType);
        builder.Property(v => v.EntityType)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnOrder(2);

        builder.Property(v => v.EntityId).IsRequired().HasColumnOrder(3);

        builder.HasIndex(v => v.ViewedAt);
        builder.Property(v => v.ViewedAt).IsRequired().HasColumnOrder(4);

        builder.Property(v => v.DurationSeconds).HasColumnOrder(5);

        builder.HasOne(v => v.User).WithMany().HasForeignKey(v => v.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}
