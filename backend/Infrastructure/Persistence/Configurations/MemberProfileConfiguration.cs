using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ManagementSystem.Infrastructure.Security;
using ManagementSystem.Modules.Family.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class MemberProfileConfiguration : IEntityTypeConfiguration<MemberProfile>
{
    private readonly string _encryptionKey;

    public MemberProfileConfiguration(IConfiguration configuration)
    {
        _encryptionKey = configuration["EncryptionSettings:TOTPKey"]
                         ?? throw new ArgumentNullException("EncryptionSettings:TOTPKey is missing!");
    }

    public void Configure(EntityTypeBuilder<MemberProfile> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnOrder(0);

        builder.HasIndex(p => p.MemberId).IsUnique();
        builder.Property(p => p.MemberId).IsRequired().HasColumnOrder(1);

        builder.Property(p => p.NationalId)
            .HasMaxLength(500)
            .IsUnicode(false)
            .HasConversion(
                v => v == null ? null : Encryptor.Encrypt(v, _encryptionKey),
                v => v == null ? null : Encryptor.Decrypt(v, _encryptionKey)
            )
            .HasColumnOrder(2);

        builder.Property(p => p.PassportNo)
            .HasMaxLength(500)
            .IsUnicode(false)
            .HasConversion(
                v => v == null ? null : Encryptor.Encrypt(v, _encryptionKey),
                v => v == null ? null : Encryptor.Decrypt(v, _encryptionKey)
            )
            .HasColumnOrder(3);

        builder.Property(p => p.Nationality).HasMaxLength(100).HasColumnOrder(4);
        builder.Property(p => p.Ethnicity).HasMaxLength(100).HasColumnOrder(5);
        builder.Property(p => p.Religion).HasMaxLength(100).HasColumnOrder(6);
        builder.Property(p => p.BloodType).HasMaxLength(10).HasColumnOrder(7);

        builder.Property(p => p.MaritalStatus)
            .HasConversion<int?>()
            .HasColumnOrder(8);

        builder.Property(p => p.EducationLevel)
            .HasConversion<int?>()
            .HasColumnOrder(9);

        builder.Property(p => p.Occupation).HasMaxLength(200).IsUnicode(true).HasColumnOrder(10);
        builder.Property(p => p.BirthPlace).HasMaxLength(300).IsUnicode(true).HasColumnOrder(11);
        builder.Property(p => p.CurrentAddress).HasMaxLength(500).IsUnicode(true).HasColumnOrder(12);
        builder.Property(p => p.PermanentAddress).HasMaxLength(500).IsUnicode(true).HasColumnOrder(13);

        builder.Property(p => p.HeightCm).HasPrecision(5, 2).HasColumnOrder(14);
        builder.Property(p => p.WeightKg).HasPrecision(5, 2).HasColumnOrder(15);

        builder.Property(p => p.EmergencyContactName)
            .HasMaxLength(200)
            .IsUnicode(true)
            .HasColumnOrder(16);
        builder.Property(p => p.EmergencyContactPhone)
            .HasMaxLength(20)
            .HasColumnOrder(17);

        builder.Property(p => p.Bio)
            .HasColumnType("text")
            .HasColumnOrder(18);

        // Audit + soft-delete
        builder.Property(p => p.CreatedAt).IsRequired().HasColumnOrder(19);
        builder.Property(p => p.UpdatedAt).IsRequired().HasColumnOrder(20);
        builder.Property(p => p.DeletedAt).HasColumnOrder(21);
        builder.Property(p => p.IsDeleted).IsRequired().HasDefaultValue(false).HasColumnOrder(22);

        // Member 1:1 relationship configured from FamilyMemberConfiguration
    }
}
