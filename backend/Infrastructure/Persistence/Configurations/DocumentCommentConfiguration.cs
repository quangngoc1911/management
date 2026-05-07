using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class DocumentCommentConfiguration : IEntityTypeConfiguration<DocumentComment>
{
    public void Configure(EntityTypeBuilder<DocumentComment> builder)
    {
        builder.HasKey(dc => dc.Id);

        builder.Property(dc => dc.Content)
            .IsRequired()
            .HasMaxLength(2000);
    }
}
