using System.Linq;
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using ManagementSystem.Domain.Entities;
using ManagementSystem.Modules.Auth.Domain.Entities;
using ManagementSystem.Modules.Categories.Domain.Entities;
using ManagementSystem.Modules.Documents.Domain.Entities;
using ManagementSystem.Modules.Family.Domain.Entities;
using ManagementSystem.Modules.Finance.Domain.Entities;
using ManagementSystem.Modules.Medical.Domain.Entities;
using ManagementSystem.Modules.Education.Domain.Entities;
using ManagementSystem.Modules.Events.Domain.Entities;
using ManagementSystem.Modules.Assets.Domain.Entities;
using ManagementSystem.Modules.Utility.Domain.Entities;
using ManagementSystem.Infrastructure.Persistence.Configurations;

namespace ManagementSystem.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    // Auth module
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<UserSession> UserSessions => Set<UserSession>();
    public DbSet<PasswordHistory> PasswordHistories => Set<PasswordHistory>();
    public DbSet<LoginAttempt> LoginAttempts => Set<LoginAttempt>();
    public DbSet<SecurityLog> SecurityLogs => Set<SecurityLog>();

    // Family module
    public DbSet<FamilyMember> FamilyMembers => Set<FamilyMember>();
    public DbSet<MemberProfile> MemberProfiles => Set<MemberProfile>();
    public DbSet<MemberRelationship> MemberRelationships => Set<MemberRelationship>();

    // Documents module
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<DocumentTag> DocumentTags => Set<DocumentTag>();
    public DbSet<DocumentVersion> DocumentVersions => Set<DocumentVersion>();
    public DbSet<DocumentFile> Files => Set<DocumentFile>();

    // Finance module
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Budget> Budgets => Set<Budget>();
    public DbSet<Investment> Investments => Set<Investment>();
    public DbSet<RecurringTransaction> RecurringTransactions => Set<RecurringTransaction>();

    // Medical module
    public DbSet<MedicalRecord> MedicalRecords => Set<MedicalRecord>();
    public DbSet<Medication> Medications => Set<Medication>();
    public DbSet<HealthMetric> HealthMetrics => Set<HealthMetric>();

    // Education module
    public DbSet<EducationRecord> EducationRecords => Set<EducationRecord>();
    public DbSet<StudySchedule> StudySchedules => Set<StudySchedule>();

    // Events module
    public DbSet<FamilyEvent> FamilyEvents => Set<FamilyEvent>();
    public DbSet<EventMedia> EventMedia => Set<EventMedia>();

    // Assets module
    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<AssetValuation> AssetValuations => Set<AssetValuation>();

    // Utility module
    public DbSet<Reminder> Reminders => Set<Reminder>();
    public DbSet<Bookmark> Bookmarks => Set<Bookmark>();
    public DbSet<ViewHistory> ViewHistories => Set<ViewHistory>();

    // System module
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<SystemConfig> SystemConfigs => Set<SystemConfig>();
    public DbSet<BackupLog> BackupLogs => Set<BackupLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureEntities(modelBuilder);
        ConfigureGlobalSoftDeleteFilter(modelBuilder);

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.SetTableName(ToSnakeCase(entity.GetTableName()!));

            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(ToSnakeCase(property.GetColumnName()));
            }

            foreach (var key in entity.GetForeignKeys())
            {
                key.SetConstraintName(ToSnakeCase(key.GetConstraintName()!));
            }
        }
    }

    private void ConfigureEntities(ModelBuilder modelBuilder)
    {
        // Auth
        modelBuilder.ApplyConfiguration(new UserConfiguration(_configuration));
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new PermissionConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
        modelBuilder.ApplyConfiguration(new RolePermissionConfiguration());
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        modelBuilder.ApplyConfiguration(new UserSessionConfiguration());
        modelBuilder.ApplyConfiguration(new PasswordHistoryConfiguration());
        modelBuilder.ApplyConfiguration(new LoginAttemptConfiguration());
        modelBuilder.ApplyConfiguration(new SecurityLogConfiguration());

        // Family
        modelBuilder.ApplyConfiguration(new FamilyMemberConfiguration());
        modelBuilder.ApplyConfiguration(new MemberProfileConfiguration(_configuration));
        modelBuilder.ApplyConfiguration(new MemberRelationshipConfiguration());

        // Documents
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentConfiguration());
        modelBuilder.ApplyConfiguration(new TagConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentTagConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentVersionConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentFileConfiguration());

        // Finance
        modelBuilder.ApplyConfiguration(new AccountConfiguration(_configuration));
        modelBuilder.ApplyConfiguration(new TransactionConfiguration());
        modelBuilder.ApplyConfiguration(new BudgetConfiguration());
        modelBuilder.ApplyConfiguration(new InvestmentConfiguration());
        modelBuilder.ApplyConfiguration(new RecurringTransactionConfiguration());

        // Medical
        modelBuilder.ApplyConfiguration(new MedicalRecordConfiguration());
        modelBuilder.ApplyConfiguration(new MedicationConfiguration());
        modelBuilder.ApplyConfiguration(new HealthMetricConfiguration());

        // Education
        modelBuilder.ApplyConfiguration(new EducationRecordConfiguration());
        modelBuilder.ApplyConfiguration(new StudyScheduleConfiguration());

        // Events
        modelBuilder.ApplyConfiguration(new FamilyEventConfiguration());
        modelBuilder.ApplyConfiguration(new EventMediaConfiguration());

        // Assets
        modelBuilder.ApplyConfiguration(new AssetConfiguration());
        modelBuilder.ApplyConfiguration(new AssetValuationConfiguration());

        // Utility
        modelBuilder.ApplyConfiguration(new ReminderConfiguration());
        modelBuilder.ApplyConfiguration(new BookmarkConfiguration());
        modelBuilder.ApplyConfiguration(new ViewHistoryConfiguration());

        // System
        modelBuilder.ApplyConfiguration(new NotificationConfiguration());
        modelBuilder.ApplyConfiguration(new AuditLogConfiguration());
        modelBuilder.ApplyConfiguration(new SystemConfigConfiguration());
        modelBuilder.ApplyConfiguration(new BackupLogConfiguration());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditFields();
        return await base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        ApplyAuditFields();
        return base.SaveChanges();
    }

    private void ApplyAuditFields()
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
                entry.Entity.IsDeleted = false;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
            }

            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedAt = now;
                entry.Entity.UpdatedAt = now;
            }
        }
    }

    private static void ConfigureGlobalSoftDeleteFilter(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                continue;

            var isDeletedProp = entityType.FindProperty(nameof(BaseEntity.IsDeleted));
            if (isDeletedProp == null)
                continue;

            var parameter = Expression.Parameter(entityType.ClrType, "e");
            var property = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
            var body = Expression.Equal(property, Expression.Constant(false));
            var lambda = Expression.Lambda(body, parameter);

            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
        }
    }

    private static string ToSnakeCase(string name)
    {
        return string.Concat(
            name.Select((c, i) => i > 0 && char.IsUpper(c) ? "_" + c : c.ToString())
        ).ToLower();
    }
}
