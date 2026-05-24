using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ManagementSystem.Modules.Education.Domain.Entities;

namespace ManagementSystem.Infrastructure.Persistence.Configurations;

public class StudyScheduleConfiguration : IEntityTypeConfiguration<StudySchedule>
{
    public void Configure(EntityTypeBuilder<StudySchedule> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnOrder(0);

        builder.HasIndex(s => s.MemberId);
        builder.Property(s => s.MemberId).IsRequired().HasColumnOrder(1);

        builder.Property(s => s.EducationRecordId).HasColumnOrder(2);

        builder.Property(s => s.Title).IsRequired().HasMaxLength(255).IsUnicode(true).HasColumnOrder(3);
        builder.Property(s => s.Subject).HasMaxLength(255).IsUnicode(true).HasColumnOrder(4);
        builder.Property(s => s.StartTime).IsRequired().HasColumnOrder(5);
        builder.Property(s => s.EndTime).IsRequired().HasColumnOrder(6);
        builder.Property(s => s.RecurrenceRule).HasColumnType("text").HasColumnOrder(7);
        builder.Property(s => s.Location).HasMaxLength(500).IsUnicode(true).HasColumnOrder(8);
        builder.Property(s => s.IsOnline).HasColumnOrder(9);
        builder.Property(s => s.MeetingUrl).HasColumnType("text").HasColumnOrder(10);
        builder.Property(s => s.TeacherName).HasMaxLength(255).IsUnicode(true).HasColumnOrder(11);

        builder.Property(s => s.Status).IsRequired().HasConversion<int>().HasColumnOrder(12);

        builder.Property(s => s.CreatedAt).IsRequired().HasColumnOrder(13);
        builder.Property(s => s.UpdatedAt).IsRequired().HasColumnOrder(14);

        builder.HasOne(s => s.Member).WithMany().HasForeignKey(s => s.MemberId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(s => s.EducationRecord).WithMany(er => er.StudySchedules).HasForeignKey(s => s.EducationRecordId).OnDelete(DeleteBehavior.SetNull);
    }
}
