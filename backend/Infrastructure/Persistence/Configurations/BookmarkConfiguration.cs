using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class BookmarkConfiguration : IEntityTypeConfiguration<Bookmark>
{
    public void Configure(EntityTypeBuilder<Bookmark> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Note)
            .HasMaxLength(500);
    }
}
