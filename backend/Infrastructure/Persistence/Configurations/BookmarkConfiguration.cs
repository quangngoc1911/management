using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Modules.Utility.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class BookmarkConfiguration : IEntityTypeConfiguration<Bookmark>
{
    public void Configure(EntityTypeBuilder<Bookmark> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).HasColumnOrder(0);

        builder.HasIndex(b => b.UserId);
        builder.Property(b => b.UserId).IsRequired().HasColumnOrder(1);

        builder.Property(b => b.EntityType)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnOrder(2);

        builder.Property(b => b.EntityId).IsRequired().HasColumnOrder(3);

        builder.Property(b => b.Note).HasColumnType("text").HasColumnOrder(4);

        builder.Property(b => b.CreatedAt).IsRequired().HasColumnOrder(5);

        builder.HasIndex(b => new { b.UserId, b.EntityType, b.EntityId }).IsUnique();

        builder.HasOne(b => b.User).WithMany().HasForeignKey(b => b.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}
