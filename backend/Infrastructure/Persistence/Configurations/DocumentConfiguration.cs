using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Modules.Documents.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).HasColumnOrder(0);

        builder.Property(d => d.Title)
            .IsRequired()
            .HasMaxLength(500)
            .IsUnicode(true)
            .HasColumnOrder(1);

        builder.HasIndex(d => d.Slug).IsUnique();
        builder.Property(d => d.Slug)
            .IsRequired()
            .HasMaxLength(500)
            .HasColumnOrder(2);

        builder.Property(d => d.Summary)
            .HasMaxLength(2000)
            .HasColumnOrder(3);

        builder.HasIndex(d => d.ContentType);
        builder.Property(d => d.ContentType)
            .IsRequired()
            .HasMaxLength(10)
            .HasColumnOrder(4);

        builder.Property(d => d.Content)
            .HasColumnType("text")
            .HasColumnOrder(5);

        builder.Property(d => d.FileId).HasColumnOrder(6);

        builder.Property(d => d.ThumbnailUrl)
            .HasMaxLength(500)
            .HasColumnOrder(7);

        builder.HasIndex(d => d.CategoryId);
        builder.Property(d => d.CategoryId).IsRequired().HasColumnOrder(8);

        builder.Property(d => d.MemberId).HasColumnOrder(9);

        builder.HasIndex(d => d.CreatedByUserId);
        builder.Property(d => d.CreatedByUserId).IsRequired().HasColumnOrder(10);

        builder.HasIndex(d => d.IsPublished);
        builder.Property(d => d.IsPublished).IsRequired().HasDefaultValue(false).HasColumnOrder(11);

        builder.Property(d => d.PublishedAt).HasColumnOrder(12);

        builder.Property(d => d.ViewCount).IsRequired().HasDefaultValue(0).HasColumnOrder(13);
        builder.Property(d => d.SortOrder).IsRequired().HasDefaultValue(0).HasColumnOrder(14);
        builder.Property(d => d.Version).IsRequired().HasDefaultValue(1).HasColumnOrder(15);

        builder.Property(d => d.CreatedAt).IsRequired().HasColumnOrder(16);
        builder.Property(d => d.UpdatedAt).HasColumnOrder(17);
        builder.Property(d => d.IsDeleted).IsRequired().HasDefaultValue(false).HasColumnOrder(18);

        // Relationships
        builder.HasOne(d => d.File)
            .WithMany(f => f.Documents)
            .HasForeignKey(d => d.FileId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(d => d.Member)
            .WithMany(m => m.Documents)
            .HasForeignKey(d => d.MemberId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(d => d.DocumentTags)
            .WithOne(dt => dt.Document)
            .HasForeignKey(dt => dt.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.Versions)
            .WithOne(dv => dv.Document)
            .HasForeignKey(dv => dv.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
