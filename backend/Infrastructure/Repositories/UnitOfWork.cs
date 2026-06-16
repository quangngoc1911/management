using ManagementSystem.Application.Contracts;
using ManagementSystem.Infrastructure.Persistence;
using ManagementSystem.Modules.Categories.Infrastructure.Repositories;
using ManagementSystem.Modules.Documents.Infrastructure.Repositories;
using ManagementSystem.Modules.Users.Infrastructure.Repositories;
using ManagementSystem.Modules.Family.Infrastructure.Repositories;
using ManagementSystem.Modules.Finance.Infrastructure.Repositories;
using ManagementSystem.Modules.Medical.Infrastructure.Repositories;
using ManagementSystem.Modules.Education.Infrastructure.Repositories;
using ManagementSystem.Modules.Events.Infrastructure.Repositories;
using ManagementSystem.Modules.Assets.Infrastructure.Repositories;
using ManagementSystem.Modules.Utility.Infrastructure.Repositories;
using ManagementSystem.Modules.SystemAdmin.Infrastructure.Repositories;
// TagRepository lives in the Documents module (Tag is part of the Documents domain).

namespace ManagementSystem.Infrastructure.Repositories;

/// <summary>
/// Unit of Work implementation for managing database operations
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDocumentRepository? _documents;
    private ICategoryRepository? _categories;
    private ITagRepository? _tags;
    private IUserRepository? _users;
    private IFamilyMemberRepository? _familyMembers;
    private IMemberProfileRepository? _memberProfiles;
    private IMemberRelationshipRepository? _relationships;
    private IAccountRepository? _accounts;
    private ITransactionRepository? _transactions;
    private IBudgetRepository? _budgets;
    private IInvestmentRepository? _investments;
    private IRecurringTransactionRepository? _recurringTransactions;
    private IMedicalRecordRepository? _medicalRecords;
    private IMedicationRepository? _medications;
    private IHealthMetricRepository? _healthMetrics;
    private IEducationRecordRepository? _educationRecords;
    private IStudyScheduleRepository? _studySchedules;
    private IFamilyEventRepository? _familyEvents;
    private IEventMediaRepository? _eventMedia;
    private IAssetRepository? _assets;
    private IAssetValuationRepository? _assetValuations;
    private IReminderRepository? _reminders;
    private IBookmarkRepository? _bookmarks;
    private IViewHistoryRepository? _viewHistories;
    private INotificationRepository? _notifications;
    private ISystemConfigRepository? _systemConfigs;
    private IAuditLogRepository? _auditLogs;
    private IBackupLogRepository? _backupLogs;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IDocumentRepository Documents => _documents ??= new DocumentRepository(_context);

    public ICategoryRepository Categories => _categories ??= new CategoryRepository(_context);

    public ITagRepository Tags => _tags ??= new TagRepository(_context);

    public IUserRepository Users => _users ??= new UserRepository(_context);

    public IFamilyMemberRepository FamilyMembers => _familyMembers ??= new FamilyMemberRepository(_context);

    public IMemberProfileRepository MemberProfiles => _memberProfiles ??= new MemberProfileRepository(_context);

    public IMemberRelationshipRepository Relationships => _relationships ??= new MemberRelationshipRepository(_context);

    public IAccountRepository Accounts => _accounts ??= new AccountRepository(_context);

    public ITransactionRepository Transactions => _transactions ??= new TransactionRepository(_context);

    public IBudgetRepository Budgets => _budgets ??= new BudgetRepository(_context);

    public IInvestmentRepository Investments => _investments ??= new InvestmentRepository(_context);

    public IRecurringTransactionRepository RecurringTransactions => _recurringTransactions ??= new RecurringTransactionRepository(_context);

    public IMedicalRecordRepository MedicalRecords => _medicalRecords ??= new MedicalRecordRepository(_context);

    public IMedicationRepository Medications => _medications ??= new MedicationRepository(_context);

    public IHealthMetricRepository HealthMetrics => _healthMetrics ??= new HealthMetricRepository(_context);

    public IEducationRecordRepository EducationRecords => _educationRecords ??= new EducationRecordRepository(_context);

    public IStudyScheduleRepository StudySchedules => _studySchedules ??= new StudyScheduleRepository(_context);

    public IFamilyEventRepository FamilyEvents => _familyEvents ??= new FamilyEventRepository(_context);

    public IEventMediaRepository EventMedia => _eventMedia ??= new EventMediaRepository(_context);

    public IAssetRepository Assets => _assets ??= new AssetRepository(_context);

    public IAssetValuationRepository AssetValuations => _assetValuations ??= new AssetValuationRepository(_context);

    public IReminderRepository Reminders => _reminders ??= new ReminderRepository(_context);

    public IBookmarkRepository Bookmarks => _bookmarks ??= new BookmarkRepository(_context);

    public IViewHistoryRepository ViewHistories => _viewHistories ??= new ViewHistoryRepository(_context);

    public INotificationRepository Notifications => _notifications ??= new NotificationRepository(_context);

    public ISystemConfigRepository SystemConfigs => _systemConfigs ??= new SystemConfigRepository(_context);

    public IAuditLogRepository AuditLogs => _auditLogs ??= new AuditLogRepository(_context);

    public IBackupLogRepository BackupLogs => _backupLogs ??= new BackupLogRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
