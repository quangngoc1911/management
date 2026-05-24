using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Modules.Auth.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(rt => rt.Id);
        builder.Property(rt => rt.Id).HasColumnOrder(0);

        builder.HasIndex(rt => rt.UserId);
        builder.Property(rt => rt.UserId).IsRequired().HasColumnOrder(1);

        builder.HasIndex(rt => rt.TokenHash).IsUnique();
        builder.Property(rt => rt.TokenHash)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnOrder(2);

        builder.Property(rt => rt.ExpiresAt).IsRequired().HasColumnOrder(3);
        builder.Property(rt => rt.RevokedAt).HasColumnOrder(4);

        builder.Property(rt => rt.CreatedAt).IsRequired().HasColumnOrder(5);
    }
}
