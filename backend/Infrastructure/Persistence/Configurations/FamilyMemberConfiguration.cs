using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Modules.Family.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class FamilyMemberConfiguration : IEntityTypeConfiguration<FamilyMember>
{
    public void Configure(EntityTypeBuilder<FamilyMember> builder)
    {
        builder.HasKey(fm => fm.Id);
        builder.Property(fm => fm.Id).HasColumnOrder(0);

        builder.HasIndex(fm => fm.UserId);
        builder.Property(fm => fm.UserId).HasColumnOrder(1);

        builder.HasIndex(fm => fm.FullName);
        builder.Property(fm => fm.FullName)
            .IsRequired()
            .HasMaxLength(200)
            .IsUnicode(true)
            .HasColumnOrder(2);

        builder.Property(fm => fm.Nickname)
            .HasMaxLength(100)
            .IsUnicode(true)
            .HasColumnOrder(3);

        builder.Property(fm => fm.DateOfBirth).HasColumnOrder(4);
        builder.Property(fm => fm.DateOfDeath).HasColumnOrder(5);

        builder.Property(fm => fm.Gender)
            .HasConversion<int?>()
            .HasColumnOrder(6);

        builder.Property(fm => fm.AvatarUrl)
            .HasMaxLength(500)
            .HasColumnOrder(7);

        builder.Property(fm => fm.Phone)
            .HasMaxLength(20)
            .HasColumnOrder(8);

        builder.Property(fm => fm.Email)
            .HasMaxLength(255)
            .HasColumnOrder(9);

        builder.Property(fm => fm.RelationToHead)
            .HasConversion<int?>()
            .HasColumnOrder(10);

        builder.Property(fm => fm.IsHouseholdHead)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnOrder(11);

        builder.HasIndex(fm => new { fm.Status, fm.IsHouseholdHead });
        builder.Property(fm => fm.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasColumnOrder(12);

        builder.Property(fm => fm.Notes)
            .HasColumnType("text")
            .HasColumnOrder(13);

        // Audit + soft-delete (opt-in per project convention)
        builder.Property(fm => fm.CreatedAt).IsRequired().HasColumnOrder(14);
        builder.Property(fm => fm.UpdatedAt).IsRequired().HasColumnOrder(15);
        builder.Property(fm => fm.DeletedAt).HasColumnOrder(16);
        builder.Property(fm => fm.IsDeleted).IsRequired().HasDefaultValue(false).HasColumnOrder(17);

        // Relationships
        builder.HasOne(fm => fm.User)
            .WithMany()
            .HasForeignKey(fm => fm.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(fm => fm.Profile)
            .WithOne(p => p.Member)
            .HasForeignKey<MemberProfile>(p => p.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(fm => fm.RelationshipsAsSource)
            .WithOne(r => r.Member)
            .HasForeignKey(r => r.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(fm => fm.RelationshipsAsTarget)
            .WithOne(r => r.RelatedMember)
            .HasForeignKey(r => r.RelatedMemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
