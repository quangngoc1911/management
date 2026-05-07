using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.HasKey(al => al.Id);

        builder.Property(al => al.Action)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(al => al.EntityName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(al => al.OldValues)
            .HasColumnType("text");

        builder.Property(al => al.NewValues)
            .HasColumnType("text");

        builder.Property(al => al.IpAddress)
            .HasMaxLength(45); // IPv6

        builder.Property(al => al.UserAgent)
            .HasMaxLength(500);
    }
}
