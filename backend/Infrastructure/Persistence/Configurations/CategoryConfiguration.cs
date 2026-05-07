using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.Categories.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Slug)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(c => c.Slug)
            .IsUnique();

        builder.Property(c => c.Description)
            .HasMaxLength(500);

        builder.Property(c => c.Icon)
            .HasMaxLength(100);

        builder.Property(c => c.CoverImageUrl)
            .HasMaxLength(500);

        // Self-referencing relationship for hierarchy
        builder.HasOne(c => c.Parent)
            .WithMany(c => c.Children)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relationships
        builder.HasMany(c => c.Documents)
            .WithOne(d => d.Category)
            .HasForeignKey(d => d.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

