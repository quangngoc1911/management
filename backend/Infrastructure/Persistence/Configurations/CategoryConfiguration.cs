using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Modules.Categories.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnOrder(0);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200)
            .IsUnicode(true)
            .HasColumnOrder(1);

        builder.HasIndex(c => c.Slug).IsUnique();
        builder.Property(c => c.Slug)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnOrder(2);

        builder.Property(c => c.Description)
            .HasMaxLength(1000)
            .HasColumnOrder(3);

        builder.Property(c => c.Icon)
            .HasMaxLength(100)
            .HasColumnOrder(4);

        builder.Property(c => c.CoverImageUrl)
            .HasMaxLength(500)
            .HasColumnOrder(5);

        builder.Property(c => c.ParentId).HasColumnOrder(6);

        builder.Property(c => c.SortOrder)
            .IsRequired()
            .HasDefaultValue(0)
            .HasColumnOrder(7);

        builder.Property(c => c.IsActive)
            .IsRequired()
            .HasDefaultValue(true)
            .HasColumnOrder(8);

        builder.Property(c => c.CreatedAt).IsRequired().HasColumnOrder(9);
        builder.Property(c => c.IsDeleted).IsRequired().HasDefaultValue(false).HasColumnOrder(10);

        builder.HasOne(c => c.Parent)
            .WithMany(c => c.Children)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Documents)
            .WithOne(d => d.Category)
            .HasForeignKey(d => d.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
