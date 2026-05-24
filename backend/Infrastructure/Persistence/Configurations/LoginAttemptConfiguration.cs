using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Modules.Auth.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class LoginAttemptConfiguration : IEntityTypeConfiguration<LoginAttempt>
{
    public void Configure(EntityTypeBuilder<LoginAttempt> builder)
    {
        builder.HasKey(la => la.Id);
        builder.Property(la => la.Id).HasColumnOrder(0);

        builder.HasIndex(la => la.Identifier);
        builder.Property(la => la.Identifier)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnOrder(1);

        builder.HasIndex(la => la.UserId);
        builder.Property(la => la.UserId).HasColumnOrder(2);

        builder.HasIndex(la => la.IpAddress);
        builder.Property(la => la.IpAddress)
            .IsRequired()
            .HasColumnType("inet")
            .HasColumnOrder(3);

        builder.HasIndex(la => la.Success);
        builder.Property(la => la.Success).IsRequired().HasColumnOrder(4);

        builder.HasIndex(la => la.AttemptedAt);
        builder.Property(la => la.AttemptedAt).IsRequired().HasColumnOrder(5);
    }
}
