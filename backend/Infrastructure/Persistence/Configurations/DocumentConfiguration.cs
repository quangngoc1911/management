using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.Documents.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.Slug)
            .HasMaxLength(200);

        builder.HasIndex(d => d.Slug)
            .IsUnique()
            .HasFilter("[slug] IS NOT NULL");

        builder.Property(d => d.Description)
            .HasMaxLength(1000);

        builder.Property(d => d.Summary)
            .HasMaxLength(500);

        builder.Property(d => d.ContentType)
            .HasMaxLength(50);

        builder.Property(d => d.Status)
            .HasMaxLength(50);

        builder.Property(d => d.FilePath)
            .HasMaxLength(500);

        builder.Property(d => d.FileName)
            .HasMaxLength(255);

        builder.Property(d => d.FileType)
            .HasMaxLength(100);

        builder.Property(d => d.ThumbnailUrl)
            .HasMaxLength(500);

        // Relationships
        builder.HasMany(d => d.DocumentTags)
            .WithOne(dt => dt.Document)
            .HasForeignKey(dt => dt.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.Versions)
            .WithOne(dv => dv.Document)
            .HasForeignKey(dv => dv.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.Attachments)
            .WithOne(a => a.Document)
            .HasForeignKey(a => a.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.Comments)
            .WithOne(c => c.Document)
            .HasForeignKey(c => c.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.Shares)
            .WithOne(ds => ds.Document)
            .HasForeignKey(ds => ds.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.Favorites)
            .WithOne(f => f.Document)
            .HasForeignKey(f => f.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

