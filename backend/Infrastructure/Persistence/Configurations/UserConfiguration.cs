using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ManagementSystem.Infrastructure.Security;
using ManagementSystem.Modules.Auth.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    private readonly string _encryptionKey;

    public UserConfiguration(IConfiguration configuration)
    {
        _encryptionKey = configuration["EncryptionSettings:TOTPKey"]
                         ?? throw new ArgumentNullException("EncryptionSettings:TOTPKey is missing!");
    }

    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnOrder(0);

        builder.HasIndex(u => u.Email).IsUnique();
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnOrder(1);

        builder.HasIndex(u => u.UserName).IsUnique();
        builder.Property(u => u.UserName)
            .HasMaxLength(150)
            .IsUnicode(true)
            .HasColumnOrder(2);

        builder.Property(u => u.NormalizedUserName)
            .HasMaxLength(150)
            .HasColumnOrder(3);

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnOrder(4);

        builder.Property(u => u.Phone)
            .HasMaxLength(20)
            .HasColumnOrder(5);

        builder.Property(u => u.NormalizedPhone)
            .HasMaxLength(20)
            .HasColumnOrder(6);

        builder.Property(u => u.AvatarUrl)
            .HasMaxLength(500)
            .HasColumnOrder(7);

        builder.Property(u => u.EmailVerifiedAt)
            .HasColumnOrder(8);

        builder.Property(u => u.TwoFactorEnabled)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnOrder(9);

        builder.Property(u => u.TwoFactorSecret)
            .HasMaxLength(500)
            .IsUnicode(false)
            .HasConversion(
                v => v == null ? null : Encryptor.Encrypt(v, _encryptionKey),
                v => v == null ? null : Encryptor.Decrypt(v, _encryptionKey)
            )
            .HasColumnOrder(10);

        builder.Property(u => u.LastLoginAt)
            .HasColumnOrder(11);

        builder.Property(u => u.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasColumnOrder(12);

        builder.Property(u => u.FailedLoginCount)
            .IsRequired()
            .HasDefaultValue((short)0)
            .HasColumnOrder(13);

        builder.Property(u => u.LockedUntil)
            .HasColumnOrder(14);

        builder.Property(u => u.PasswordChangedAt)
            .HasColumnOrder(15);

        // Audit + soft-delete (opt-in via Fluent API; [NotMapped] in BaseEntity)
        builder.Property(u => u.CreatedAt).IsRequired().HasColumnOrder(16);
        builder.Property(u => u.UpdatedAt).IsRequired().HasColumnOrder(17);
        builder.Property(u => u.DeletedAt).HasColumnOrder(18);
        builder.Property(u => u.IsDeleted).IsRequired().HasDefaultValue(false).HasColumnOrder(19);

        // Relationships
        builder.HasMany(u => u.UserRoles)
            .WithOne(ur => ur.User)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.RefreshTokens)
            .WithOne(rt => rt.User)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Sessions)
            .WithOne(s => s.User)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.PasswordHistories)
            .WithOne(ph => ph.User)
            .HasForeignKey(ph => ph.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.LoginAttempts)
            .WithOne(la => la.User!)
            .HasForeignKey(la => la.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(u => u.SecurityLogs)
            .WithOne(sl => sl.User!)
            .HasForeignKey(sl => sl.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(u => u.CreatedDocuments)
            .WithOne(d => d.CreatedByUser)
            .HasForeignKey(d => d.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.EditedVersions)
            .WithOne(dv => dv.EditedByUser)
            .HasForeignKey(dv => dv.EditedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.AuditLogs)
            .WithOne(al => al.User)
            .HasForeignKey(al => al.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
