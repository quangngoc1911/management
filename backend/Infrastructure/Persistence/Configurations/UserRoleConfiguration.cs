using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Modules.Auth.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(ur => ur.Id);
        builder.Property(ur => ur.Id).HasColumnOrder(0);

        builder.HasIndex(ur => new { ur.UserId, ur.RoleId }).IsUnique();

        builder.Property(ur => ur.UserId).IsRequired().HasColumnOrder(1);
        builder.Property(ur => ur.RoleId).IsRequired().HasColumnOrder(2);
        builder.Property(ur => ur.GrantedBy).HasColumnOrder(3);
        builder.Property(ur => ur.ExpiresAt).HasColumnOrder(4);
        builder.Property(ur => ur.RevokedAt).HasColumnOrder(5);

        // Audit (created only — no soft-delete; revoked_at used for revocation)
        builder.Property(ur => ur.CreatedAt).IsRequired().HasColumnOrder(6);

        // Granter — second FK to User via GrantedBy (no inverse collection)
        builder.HasOne(ur => ur.Granter)
            .WithMany()
            .HasForeignKey(ur => ur.GrantedBy)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
