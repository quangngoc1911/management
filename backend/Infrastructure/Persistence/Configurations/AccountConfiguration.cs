using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ManagementSystem.Infrastructure.Security;
using ManagementSystem.Modules.Finance.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    private readonly string _encryptionKey;

    public AccountConfiguration(IConfiguration configuration)
    {
        _encryptionKey = configuration["EncryptionSettings:TOTPKey"]
                         ?? throw new ArgumentNullException("EncryptionSettings:TOTPKey is missing!");
    }

    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnOrder(0);

        builder.HasIndex(a => a.MemberId);
        builder.Property(a => a.MemberId).HasColumnOrder(1);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(255)
            .IsUnicode(true)
            .HasColumnOrder(2);

        builder.HasIndex(a => a.AccountType);
        builder.Property(a => a.AccountType)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnOrder(3);

        builder.Property(a => a.AccountNumber)
            .HasMaxLength(500)
            .IsUnicode(false)
            .HasConversion(
                v => v == null ? null : Encryptor.Encrypt(v, _encryptionKey),
                v => v == null ? null : Encryptor.Decrypt(v, _encryptionKey)
            )
            .HasColumnOrder(4);

        builder.Property(a => a.BankName)
            .HasMaxLength(100)
            .HasColumnOrder(5);

        builder.Property(a => a.Currency)
            .IsRequired()
            .HasMaxLength(3)
            .IsFixedLength()
            .HasColumnOrder(6);

        builder.Property(a => a.IsActive)
            .IsRequired()
            .HasDefaultValue(true)
            .HasColumnOrder(7);

        builder.Property(a => a.CreatedAt).IsRequired().HasColumnOrder(8);
        builder.Property(a => a.UpdatedAt).IsRequired().HasColumnOrder(9);

        builder.HasOne(a => a.Member)
            .WithMany()
            .HasForeignKey(a => a.MemberId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
