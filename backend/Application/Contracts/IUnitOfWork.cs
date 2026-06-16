namespace ManagementSystem.Application.Contracts;

/// <summary>
/// Unit of Work pattern interface for managing database operations
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IDocumentRepository Documents { get; }

    ICategoryRepository Categories { get; }

    ITagRepository Tags { get; }

    IUserRepository Users { get; }

    IFamilyMemberRepository FamilyMembers { get; }

    IMemberProfileRepository MemberProfiles { get; }

    IMemberRelationshipRepository Relationships { get; }

    IAccountRepository Accounts { get; }

    ITransactionRepository Transactions { get; }

    IBudgetRepository Budgets { get; }

    IInvestmentRepository Investments { get; }

    IRecurringTransactionRepository RecurringTransactions { get; }

    IMedicalRecordRepository MedicalRecords { get; }

    IMedicationRepository Medications { get; }

    IHealthMetricRepository HealthMetrics { get; }

    IEducationRecordRepository EducationRecords { get; }

    IStudyScheduleRepository StudySchedules { get; }

    IFamilyEventRepository FamilyEvents { get; }

    IEventMediaRepository EventMedia { get; }

    IAssetRepository Assets { get; }

    IAssetValuationRepository AssetValuations { get; }

    IReminderRepository Reminders { get; }

    IBookmarkRepository Bookmarks { get; }

    IViewHistoryRepository ViewHistories { get; }

    INotificationRepository Notifications { get; }

    ISystemConfigRepository SystemConfigs { get; }

    IAuditLogRepository AuditLogs { get; }

    IBackupLogRepository BackupLogs { get; }

    Task<int> SaveChangesAsync();
}
