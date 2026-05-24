using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Modules.Education.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class EducationRecordConfiguration : IEntityTypeConfiguration<EducationRecord>
{
    public void Configure(EntityTypeBuilder<EducationRecord> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnOrder(0);

        builder.HasIndex(e => e.MemberId);
        builder.Property(e => e.MemberId).IsRequired().HasColumnOrder(1);

        builder.Property(e => e.InstitutionName).IsRequired().HasMaxLength(500).IsUnicode(true).HasColumnOrder(2);
        builder.Property(e => e.Level).IsRequired().HasMaxLength(100).HasColumnOrder(3);
        builder.Property(e => e.Major).HasMaxLength(255).IsUnicode(true).HasColumnOrder(4);
        builder.Property(e => e.Degree).HasMaxLength(255).IsUnicode(true).HasColumnOrder(5);
        builder.Property(e => e.StudentId).HasMaxLength(100).HasColumnOrder(6);
        builder.Property(e => e.StartDate).HasColumnOrder(7);
        builder.Property(e => e.EndDate).HasColumnOrder(8);
        builder.Property(e => e.Gpa).HasPrecision(4, 2).HasColumnOrder(9);

        builder.Property(e => e.Status).IsRequired().HasConversion<int>().HasColumnOrder(10);

        builder.Property(e => e.Achievements).HasColumnType("jsonb").HasColumnOrder(11);
        builder.Property(e => e.Notes).HasColumnType("text").HasColumnOrder(12);

        builder.Property(e => e.CreatedAt).IsRequired().HasColumnOrder(13);
        builder.Property(e => e.UpdatedAt).IsRequired().HasColumnOrder(14);

        builder.HasOne(e => e.Member).WithMany().HasForeignKey(e => e.MemberId).OnDelete(DeleteBehavior.Restrict);
    }
}
