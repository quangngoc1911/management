using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.Documents.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class DocumentFieldConfiguration : IEntityTypeConfiguration<DocumentField>
{
    public void Configure(EntityTypeBuilder<DocumentField> builder)
    {
        builder.HasKey(df => df.Id);

        builder.Property(df => df.FieldName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(df => df.FieldValue)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(df => df.FieldType)
            .HasMaxLength(50);

        builder.Property(df => df.SortOrder)
            .IsRequired();
    }
}

