using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Modules.Auth.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnOrder(0);

        builder.HasIndex(s => s.UserId);
        builder.Property(s => s.UserId).IsRequired().HasColumnOrder(1);

        builder.HasIndex(s => s.SessionToken).IsUnique();
        builder.Property(s => s.SessionToken)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnOrder(2);

        builder.Property(s => s.LastActiveAt).IsRequired().HasColumnOrder(3);
        builder.Property(s => s.ExpiresAt).IsRequired().HasColumnOrder(4);

        builder.Property(s => s.CreatedAt).IsRequired().HasColumnOrder(5);
    }
}
