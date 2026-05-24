using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Modules.Auth.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class SecurityLogConfiguration : IEntityTypeConfiguration<SecurityLog>
{
    public void Configure(EntityTypeBuilder<SecurityLog> builder)
    {
        builder.HasKey(sl => sl.Id);
        builder.Property(sl => sl.Id).HasColumnOrder(0);

        builder.HasIndex(sl => sl.UserId);
        builder.Property(sl => sl.UserId).HasColumnOrder(1);

        builder.HasIndex(sl => sl.EventType);
        builder.Property(sl => sl.EventType)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnOrder(2);

        builder.HasIndex(sl => sl.IpAddress);
        builder.Property(sl => sl.IpAddress)
            .IsRequired()
            .HasColumnType("inet")
            .HasColumnOrder(3);

        builder.Property(sl => sl.RiskLevel)
            .HasConversion<int?>()
            .HasMaxLength(20)
            .HasColumnOrder(4);

        builder.Property(sl => sl.UserAgent)
            .HasColumnType("text")
            .HasColumnOrder(5);

        builder.Property(sl => sl.Browser)
            .HasMaxLength(50)
            .HasColumnOrder(6);

        builder.HasIndex(sl => sl.Os);
        builder.Property(sl => sl.Os)
            .HasMaxLength(50)
            .HasColumnOrder(7);

        builder.HasIndex(sl => sl.DeviceType);
        builder.Property(sl => sl.DeviceType)
            .HasConversion<int?>()
            .HasMaxLength(30)
            .HasColumnOrder(8);

        builder.Property(sl => sl.EventStatus)
            .HasColumnName("status")
            .HasConversion<int?>()
            .HasColumnOrder(9);

        builder.HasIndex(sl => sl.CreatedAt);
        builder.Property(sl => sl.CreatedAt).IsRequired().HasColumnOrder(10);
    }
}
