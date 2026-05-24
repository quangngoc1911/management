using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.HasKey(al => al.Id);
        builder.Property(al => al.Id).HasColumnOrder(0);

        builder.HasIndex(al => al.UserId);
        builder.Property(al => al.UserId).HasColumnOrder(1);

        builder.HasIndex(al => al.Action);
        builder.Property(al => al.Action).IsRequired().HasConversion<int>().HasColumnOrder(2);

        builder.HasIndex(al => al.EntityType);
        builder.Property(al => al.EntityType).IsRequired().HasMaxLength(100).HasColumnOrder(3);

        builder.Property(al => al.EntityId).HasColumnOrder(4);

        builder.Property(al => al.OldValues).HasColumnType("jsonb").HasColumnOrder(5);
        builder.Property(al => al.NewValues).HasColumnType("jsonb").HasColumnOrder(6);

        builder.Property(al => al.IpAddress).HasColumnType("inet").HasColumnOrder(7);

        builder.HasIndex(al => al.CreatedAt);
        builder.Property(al => al.CreatedAt).IsRequired().HasColumnOrder(8);
    }
}
