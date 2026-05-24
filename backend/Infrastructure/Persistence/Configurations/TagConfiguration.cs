using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Modules.Documents.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).HasColumnOrder(0);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100)
            .IsUnicode(true)
            .HasColumnOrder(1);

        builder.HasIndex(t => t.Slug).IsUnique();
        builder.Property(t => t.Slug)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnOrder(2);

        builder.Property(t => t.Color)
            .HasMaxLength(7)
            .HasColumnOrder(3);

        builder.Property(t => t.CreatedAt).IsRequired().HasColumnOrder(4);
        builder.Property(t => t.IsDeleted).IsRequired().HasDefaultValue(false).HasColumnOrder(5);

        builder.HasMany(t => t.DocumentTags)
            .WithOne(dt => dt.Tag)
            .HasForeignKey(dt => dt.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
