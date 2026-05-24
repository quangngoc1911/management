using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(n => n.Id);
        builder.Property(n => n.Id).HasColumnOrder(0);

        builder.HasIndex(n => n.UserId);
        builder.Property(n => n.UserId).IsRequired().HasColumnOrder(1);

        builder.Property(n => n.Title)
            .IsRequired()
            .HasMaxLength(500)
            .IsUnicode(true)
            .HasColumnOrder(2);

        builder.Property(n => n.Body).HasColumnType("text").HasColumnOrder(3);

        builder.Property(n => n.Type).IsRequired().HasMaxLength(100).HasColumnOrder(4);
        builder.Property(n => n.Channel).IsRequired().HasMaxLength(50).HasColumnOrder(5);

        builder.Property(n => n.EntityType).HasMaxLength(100).HasColumnOrder(6);
        builder.Property(n => n.EntityId).HasColumnOrder(7);

        builder.Property(n => n.IsRead).IsRequired().HasDefaultValue(false).HasColumnOrder(8);
        builder.Property(n => n.ReadAt).HasColumnOrder(9);
        builder.Property(n => n.SentAt).HasColumnOrder(10);

        builder.Property(n => n.CreatedAt).IsRequired().HasColumnOrder(11);

        builder.HasOne(n => n.User).WithMany().HasForeignKey(n => n.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}
