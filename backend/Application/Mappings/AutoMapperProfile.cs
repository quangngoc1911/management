using AutoMapper;
using ManagementSystem.Modules.Auth.Application.DTOs;
using ManagementSystem.Modules.Categories.Application.DTOs;
using ManagementSystem.Modules.Documents.Application.DTOs;
using ManagementSystem.Modules.Users.Application.DTOs;
using ManagementSystem.Modules.Family.Application.DTOs;
using ManagementSystem.Modules.Finance.Application.DTOs;
using ManagementSystem.Modules.Medical.Application.DTOs;
using ManagementSystem.Modules.Education.Application.DTOs;
using ManagementSystem.Modules.Events.Application.DTOs;
using ManagementSystem.Modules.Assets.Application.DTOs;
using ManagementSystem.Modules.Utility.Application.DTOs;
using ManagementSystem.Modules.SystemAdmin.Application.DTOs;
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

namespace ManagementSystem.Application.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // User Mappings
        CreateMap<User, UserDto>();
        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        CreateMap<UpdateUserDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        CreateMap<RegisterRequest, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

        // Document Mappings
        CreateMap<Document, DocumentDto>();
        CreateMap<Document, DocumentListDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category!.Name))
            .ForMember(dest => dest.CreatedByUserName, opt => opt.MapFrom(src => src.CreatedByUser!.UserName));

        // Category Mappings
        CreateMap<Category, CategoryDto>();
        CreateMap<CreateCategoryDto, Category>();

        // Tag Mappings
        CreateMap<Tag, TagDto>();
        CreateMap<CreateTagDto, Tag>();
        CreateMap<UpdateTagDto, Tag>();

        // Family Member Mappings
        CreateMap<FamilyMember, FamilyMemberDto>();
        CreateMap<CreateFamilyMemberDto, FamilyMember>();
        CreateMap<UpdateFamilyMemberDto, FamilyMember>();

        // Member Profile Mappings
        CreateMap<MemberProfile, MemberProfileDto>();
        CreateMap<UpsertMemberProfileDto, MemberProfile>();

        // Member Relationship Mappings
        CreateMap<MemberRelationship, MemberRelationshipDto>()
            .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member != null ? src.Member.FullName : string.Empty))
            .ForMember(dest => dest.RelatedMemberName, opt => opt.MapFrom(src => src.RelatedMember != null ? src.RelatedMember.FullName : string.Empty));
        CreateMap<CreateMemberRelationshipDto, MemberRelationship>();
        CreateMap<UpdateMemberRelationshipDto, MemberRelationship>();

        // Finance — Account Mappings
        CreateMap<Account, AccountDto>()
            .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member != null ? src.Member.FullName : null));
        CreateMap<CreateAccountDto, Account>();
        CreateMap<UpdateAccountDto, Account>();

        // Finance — Transaction Mappings
        CreateMap<Transaction, TransactionDto>()
            .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.Account != null ? src.Account.Name : string.Empty))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
            .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member != null ? src.Member.FullName : null))
            .ForMember(dest => dest.TransferToAccountName, opt => opt.MapFrom(src => src.TransferToAccount != null ? src.TransferToAccount.Name : null));
        CreateMap<CreateTransactionDto, Transaction>();
        CreateMap<UpdateTransactionDto, Transaction>();

        // Finance — Budget Mappings
        CreateMap<Budget, BudgetDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
            .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member != null ? src.Member.FullName : null));
        CreateMap<CreateBudgetDto, Budget>();
        CreateMap<UpdateBudgetDto, Budget>();

        // Finance — Investment Mappings
        CreateMap<Investment, InvestmentDto>()
            .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.Account != null ? src.Account.Name : null))
            .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member != null ? src.Member.FullName : null));
        CreateMap<CreateInvestmentDto, Investment>();
        CreateMap<UpdateInvestmentDto, Investment>();

        // Finance — Recurring Transaction Mappings
        CreateMap<RecurringTransaction, RecurringTransactionDto>()
            .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.Account != null ? src.Account.Name : string.Empty))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));
        CreateMap<CreateRecurringTransactionDto, RecurringTransaction>();
        CreateMap<UpdateRecurringTransactionDto, RecurringTransaction>();

        // Medical — MedicalRecord Mappings
        CreateMap<MedicalRecord, MedicalRecordDto>()
            .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member != null ? src.Member.FullName : string.Empty));
        CreateMap<CreateMedicalRecordDto, MedicalRecord>();
        CreateMap<UpdateMedicalRecordDto, MedicalRecord>();

        // Medical — Medication Mappings
        CreateMap<Medication, MedicationDto>()
            .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member != null ? src.Member.FullName : string.Empty));
        CreateMap<CreateMedicationDto, Medication>();
        CreateMap<UpdateMedicationDto, Medication>();

        // Medical — HealthMetric Mappings
        CreateMap<HealthMetric, HealthMetricDto>()
            .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member != null ? src.Member.FullName : string.Empty));
        CreateMap<CreateHealthMetricDto, HealthMetric>();
        CreateMap<UpdateHealthMetricDto, HealthMetric>();

        // Education — EducationRecord Mappings
        CreateMap<EducationRecord, EducationRecordDto>()
            .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member != null ? src.Member.FullName : string.Empty));
        CreateMap<CreateEducationRecordDto, EducationRecord>();
        CreateMap<UpdateEducationRecordDto, EducationRecord>();

        // Education — StudySchedule Mappings
        CreateMap<StudySchedule, StudyScheduleDto>()
            .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member != null ? src.Member.FullName : string.Empty));
        CreateMap<CreateStudyScheduleDto, StudySchedule>();
        CreateMap<UpdateStudyScheduleDto, StudySchedule>();

        // Events — FamilyEvent Mappings
        CreateMap<FamilyEvent, FamilyEventDto>()
            .ForMember(dest => dest.CreatedByUserName, opt => opt.MapFrom(src => src.CreatedByUser != null ? src.CreatedByUser.UserName : null));
        CreateMap<CreateFamilyEventDto, FamilyEvent>();
        CreateMap<UpdateFamilyEventDto, FamilyEvent>();

        // Events — EventMedia Mappings
        CreateMap<EventMedia, EventMediaDto>();
        CreateMap<CreateEventMediaDto, EventMedia>();
        CreateMap<UpdateEventMediaDto, EventMedia>();

        // Assets — Asset Mappings
        CreateMap<Asset, AssetDto>()
            .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member != null ? src.Member.FullName : null))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));
        CreateMap<CreateAssetDto, Asset>();
        CreateMap<UpdateAssetDto, Asset>();

        // Assets — AssetValuation Mappings
        CreateMap<AssetValuation, AssetValuationDto>()
            .ForMember(dest => dest.AssetName, opt => opt.MapFrom(src => src.Asset != null ? src.Asset.Name : string.Empty));
        CreateMap<CreateAssetValuationDto, AssetValuation>();
        CreateMap<UpdateAssetValuationDto, AssetValuation>();

        // Utility — Reminder Mappings
        CreateMap<Reminder, ReminderDto>()
            .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member != null ? src.Member.FullName : null));
        CreateMap<CreateReminderDto, Reminder>();
        CreateMap<UpdateReminderDto, Reminder>();

        // Utility — Bookmark Mappings
        CreateMap<Bookmark, BookmarkDto>();
        CreateMap<CreateBookmarkDto, Bookmark>();
        CreateMap<UpdateBookmarkDto, Bookmark>();

        // Utility — ViewHistory Mappings
        CreateMap<ViewHistory, ViewHistoryDto>();
        CreateMap<CreateViewHistoryDto, ViewHistory>();

        // System — Notification Mappings
        CreateMap<Notification, NotificationDto>();
        CreateMap<CreateNotificationDto, Notification>();

        // System — SystemConfig Mappings
        CreateMap<SystemConfig, SystemConfigDto>();
        CreateMap<CreateSystemConfigDto, SystemConfig>();
        CreateMap<UpdateSystemConfigDto, SystemConfig>();

        // System — AuditLog Mappings (read-only; IpAddress -> string)
        CreateMap<AuditLog, AuditLogDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : null))
            .ForMember(dest => dest.IpAddress, opt => opt.MapFrom(src => src.IpAddress != null ? src.IpAddress.ToString() : null));

        // System — BackupLog Mappings (read-only)
        CreateMap<BackupLog, BackupLogDto>();
    }
}
