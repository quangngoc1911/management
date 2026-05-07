using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.Documents.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.Slug)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(t => t.Slug)
            .IsUnique();

        builder.Property(t => t.Color)
            .HasMaxLength(7); // #RRGGBB

        // Relationships
        builder.HasMany(t => t.DocumentTags)
            .WithOne(dt => dt.Tag)
            .HasForeignKey(dt => dt.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

