using Microsoft.EntityFrameworkCore;

using ManagementSystem.Domain.Entities;
using ManagementSystem.Domain.Enums;
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

namespace ManagementSystem.Infrastructure.Persistence;

/// <summary>
/// Seeds baseline real data on startup. Every step is idempotent so it is safe to run on every boot.
/// Mỗi feature/module có ít nhất 5 bản ghi mẫu, được nạp theo đúng thứ tự khóa ngoại (cha trước, con sau).
/// </summary>
public static class DataSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // --- Level 0: independent parents -------------------------------------------------
        await SeedRolesAndUsersAsync(context);     // Users
        await SeedCategoriesAsync(context);        // Categories
        await SeedTagsAsync(context);              // Tags
        await SeedFamilyMembersAsync(context);     // FamilyMembers

        // --- Level 1: depend on members / users -------------------------------------------
        await SeedMemberProfilesAsync(context);    // -> FamilyMembers
        await SeedMemberRelationshipsAsync(context); // -> FamilyMembers
        await SeedDocumentFilesAsync(context);     // -> Users
        await SeedAccountsAsync(context);          // -> FamilyMembers

        // --- Level 2: documents + finance children ----------------------------------------
        await SeedDocumentsAsync(context);         // -> Categories, Users, (Files optional)
        await SeedRecurringTransactionsAsync(context); // -> Accounts, Categories
        await SeedBudgetsAsync(context);           // -> Categories, Members
        await SeedInvestmentsAsync(context);       // -> Accounts, Members

        // --- Level 3: depend on documents -------------------------------------------------
        await SeedDocumentTagsAsync(context);      // -> Documents, Tags
        await SeedDocumentVersionsAsync(context);  // -> Documents, Users
        await SeedTransactionsAsync(context);      // -> Accounts, Categories, Members

        // --- Medical / Education ----------------------------------------------------------
        await SeedMedicalRecordsAsync(context);    // -> Members, Users
        await SeedMedicationsAsync(context);       // -> Members, (MedicalRecords optional)
        await SeedHealthMetricsAsync(context);     // -> Members
        await SeedEducationRecordsAsync(context);  // -> Members
        await SeedStudySchedulesAsync(context);    // -> Members, (EducationRecords optional)

        // --- Events / Assets --------------------------------------------------------------
        await SeedFamilyEventsAsync(context);      // -> Users, (Files optional)
        await SeedEventMediaAsync(context);        // -> Events, Files, Users
        await SeedAssetsAsync(context);            // -> Members, Categories, Users
        await SeedAssetValuationsAsync(context);   // -> Assets, Users

        // --- Utility / System -------------------------------------------------------------
        await SeedRemindersAsync(context);         // -> Users, Members
        await SeedBookmarksAsync(context);         // -> Users + entity refs
        await SeedViewHistoriesAsync(context);     // -> Users + entity refs
        await SeedNotificationsAsync(context);     // -> Users
        await SeedAuditLogsAsync(context);         // -> Users (optional)
        await SeedSystemConfigsAsync(context);     // independent
        await SeedBackupLogsAsync(context);        // independent
    }

    // =====================================================================================
    // Auth
    // =====================================================================================
    private static async Task SeedRolesAndUsersAsync(ApplicationDbContext context)
    {
        // Get-or-create theo từng bản ghi để an toàn với DB đã có sẵn dữ liệu (không ghi đè).
        var adminRole = await EnsureRoleAsync(context, "Admin", "admin", "Quản trị hệ thống");
        var editorRole = await EnsureRoleAsync(context, "Editor", "editor", "Biên tập viên");
        var userRole = await EnsureRoleAsync(context, "User", "user", "Người dùng");
        await context.SaveChangesAsync();

        await EnsureUserAsync(context, "admin@example.com", "Quản trị viên", "Admin@123", adminRole);
        await EnsureUserAsync(context, "editor@example.com", "Biên tập viên", "Editor@123", editorRole);
        await EnsureUserAsync(context, "user@example.com", "Người dùng", "User@123", userRole);
        // Bổ sung thêm vài người dùng demo (role User) để mỗi feature có đủ FK người dùng.
        await EnsureUserAsync(context, "lan.tran@example.com", "Trần Thị Lan", "User@123", userRole, "0902345671");
        await EnsureUserAsync(context, "duc.pham@example.com", "Phạm Văn Đức", "User@123", userRole, "0902345672");
        await EnsureUserAsync(context, "mai.le@example.com", "Lê Thị Mai", "User@123", userRole, "0902345673");
        await context.SaveChangesAsync();
    }

    private static async Task<Role> EnsureRoleAsync(ApplicationDbContext context, string name, string slug, string description)
    {
        var role = await context.Roles.FirstOrDefaultAsync(r => r.Slug == slug);
        if (role is null)
        {
            role = new Role { Name = name, Slug = slug, Description = description, IsSystem = true };
            context.Roles.Add(role);
        }
        return role;
    }

    private static async Task EnsureUserAsync(ApplicationDbContext context, string email, string name, string password, Role role, string? phone = null)
    {
        if (await context.Users.AnyAsync(u => u.Email == email))
        {
            return;
        }

        var user = new User
        {
            Email = email,
            UserName = name,
            NormalizedUserName = name.ToUpperInvariant(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Phone = phone,
            EmailVerifiedAt = DateTime.UtcNow,
            PasswordChangedAt = DateTime.UtcNow
        };
        context.Users.Add(user);
        context.UserRoles.Add(new UserRole { User = user, Role = role });
    }

    // =====================================================================================
    // Documents - Categories & Tags
    // =====================================================================================
    private static async Task SeedCategoriesAsync(ApplicationDbContext context)
    {
        // Get-or-create theo slug để luôn đạt đủ bộ mẫu, kể cả khi bảng đã có sẵn vài bản ghi.
        var desired = new[]
        {
            new Category { Name = "Tài liệu chung", Slug = "tai-lieu-chung", Description = "Tài liệu dùng chung cho cả gia đình", Icon = "folder", SortOrder = 1 },
            new Category { Name = "Hướng dẫn", Slug = "huong-dan", Description = "Các tài liệu hướng dẫn, quy trình", Icon = "book", SortOrder = 2 },
            new Category { Name = "Chính sách", Slug = "chinh-sach", Description = "Chính sách và quy định", Icon = "shield", SortOrder = 3 },
            new Category { Name = "Giấy tờ pháp lý", Slug = "giay-to-phap-ly", Description = "Hộ khẩu, giấy khai sinh, hợp đồng", Icon = "file-text", SortOrder = 4 },
            new Category { Name = "Hình ảnh kỷ niệm", Slug = "hinh-anh-ky-niem", Description = "Album ảnh và video gia đình", Icon = "image", SortOrder = 5 },
            new Category { Name = "Sức khỏe", Slug = "suc-khoe", Description = "Hồ sơ và tài liệu sức khỏe", Icon = "heart", SortOrder = 6 }
        };

        foreach (var c in desired)
        {
            if (!await context.Categories.AnyAsync(x => x.Slug == c.Slug))
                context.Categories.Add(c);
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedTagsAsync(ApplicationDbContext context)
    {
        // Get-or-create theo slug để luôn đạt đủ bộ mẫu, kể cả khi bảng đã có sẵn vài bản ghi.
        var desired = new[]
        {
            new Tag { Name = "Quan trọng", Slug = "quan-trong", Color = "#dc2626" },
            new Tag { Name = "Công khai", Slug = "cong-khai", Color = "#16a34a" },
            new Tag { Name = "Nội bộ", Slug = "noi-bo", Color = "#2563eb" },
            new Tag { Name = "Lưu trữ", Slug = "luu-tru", Color = "#a855f7" },
            new Tag { Name = "Khẩn cấp", Slug = "khan-cap", Color = "#f97316" },
            new Tag { Name = "Tham khảo", Slug = "tham-khao", Color = "#0891b2" }
        };

        foreach (var t in desired)
        {
            if (!await context.Tags.AnyAsync(x => x.Slug == t.Slug))
                context.Tags.Add(t);
        }

        await context.SaveChangesAsync();
    }

    // =====================================================================================
    // Family
    // =====================================================================================
    private static async Task SeedFamilyMembersAsync(ApplicationDbContext context)
    {
        // Top-up theo số lượng (không có khóa tự nhiên): bổ sung cho đủ bộ mẫu, không trùng khi chạy lại.
        var existing = await context.FamilyMembers.CountAsync();
        var desired = new FamilyMember[]
        {
            new FamilyMember
            {
                FullName = "Nguyễn Văn An",
                Gender = Gender.Male,
                RelationToHead = RelationToHead.Self,
                IsHouseholdHead = true,
                Phone = "0901234567",
                Email = "an.nguyen@example.com",
                DateOfBirth = new DateOnly(1980, 5, 12),
                Notes = "Chủ hộ, làm kỹ sư xây dựng."
            },
            new FamilyMember
            {
                FullName = "Trần Thị Bình",
                Gender = Gender.Female,
                RelationToHead = RelationToHead.Spouse,
                Phone = "0901234568",
                Email = "binh.tran@example.com",
                DateOfBirth = new DateOnly(1983, 8, 20),
                Notes = "Vợ chủ hộ, giáo viên tiểu học."
            },
            new FamilyMember
            {
                FullName = "Nguyễn Văn Cường",
                Gender = Gender.Male,
                RelationToHead = RelationToHead.Child,
                DateOfBirth = new DateOnly(2010, 2, 2),
                Notes = "Con trai cả, học sinh."
            },
            new FamilyMember
            {
                FullName = "Nguyễn Thị Dung",
                Gender = Gender.Female,
                RelationToHead = RelationToHead.Child,
                DateOfBirth = new DateOnly(2014, 11, 30),
                Notes = "Con gái út, học sinh."
            },
            new FamilyMember
            {
                FullName = "Nguyễn Văn Em",
                Gender = Gender.Male,
                RelationToHead = RelationToHead.Parent,
                Phone = "0901234569",
                DateOfBirth = new DateOnly(1955, 3, 15),
                Notes = "Ông nội, đã nghỉ hưu."
            },
            new FamilyMember
            {
                FullName = "Lê Thị Hoa",
                Gender = Gender.Female,
                RelationToHead = RelationToHead.Parent,
                DateOfBirth = new DateOnly(1958, 7, 8),
                Notes = "Bà nội, nội trợ."
            }
        };

        if (existing >= desired.Length)
        {
            return;
        }

        context.FamilyMembers.AddRange(desired[existing..]);
        await context.SaveChangesAsync();
    }

    private static async Task SeedMemberProfilesAsync(ApplicationDbContext context)
    {
        if (await context.MemberProfiles.AnyAsync())
        {
            return;
        }

        var members = await context.FamilyMembers.OrderBy(m => m.DateOfBirth).ToListAsync();
        if (members.Count < 5) return; // cần ít nhất 5 thành viên cho 5 hồ sơ mẫu (tránh out-of-range)

        var profiles = new[]
        {
            new MemberProfile
            {
                MemberId = members[0].Id,
                NationalId = "001080000001",
                Nationality = "Việt Nam",
                Ethnicity = "Kinh",
                Religion = "Không",
                BloodType = "O+",
                MaritalStatus = MaritalStatus.Married,
                EducationLevel = EducationLevel.Bachelor,
                Occupation = "Kỹ sư xây dựng",
                BirthPlace = "Hà Nội",
                CurrentAddress = "Số 12, ngõ 5, phố Hàng Bài, Hoàn Kiếm, Hà Nội",
                PermanentAddress = "Số 12, ngõ 5, phố Hàng Bài, Hoàn Kiếm, Hà Nội",
                HeightCm = 172m,
                WeightKg = 68m,
                EmergencyContactName = "Trần Thị Bình",
                EmergencyContactPhone = "0901234568",
                Bio = "Chủ hộ gia đình, yêu thích đọc sách và bóng đá."
            },
            new MemberProfile
            {
                MemberId = members[1].Id,
                NationalId = "001083000002",
                Nationality = "Việt Nam",
                Ethnicity = "Kinh",
                BloodType = "A+",
                MaritalStatus = MaritalStatus.Married,
                EducationLevel = EducationLevel.Bachelor,
                Occupation = "Giáo viên",
                BirthPlace = "Nam Định",
                CurrentAddress = "Số 12, ngõ 5, phố Hàng Bài, Hoàn Kiếm, Hà Nội",
                HeightCm = 160m,
                WeightKg = 54m,
                EmergencyContactName = "Nguyễn Văn An",
                EmergencyContactPhone = "0901234567",
                Bio = "Giáo viên tiểu học, thích nấu ăn."
            },
            new MemberProfile
            {
                MemberId = members[2].Id,
                Nationality = "Việt Nam",
                Ethnicity = "Kinh",
                BloodType = "O+",
                MaritalStatus = MaritalStatus.Single,
                EducationLevel = EducationLevel.Secondary,
                Occupation = "Học sinh",
                BirthPlace = "Hà Nội",
                CurrentAddress = "Số 12, ngõ 5, phố Hàng Bài, Hoàn Kiếm, Hà Nội",
                HeightCm = 158m,
                WeightKg = 48m,
                Bio = "Học sinh lớp 8, thích chơi cờ vua."
            },
            new MemberProfile
            {
                MemberId = members[3].Id,
                Nationality = "Việt Nam",
                Ethnicity = "Kinh",
                BloodType = "A+",
                MaritalStatus = MaritalStatus.Single,
                EducationLevel = EducationLevel.Primary,
                Occupation = "Học sinh",
                BirthPlace = "Hà Nội",
                CurrentAddress = "Số 12, ngõ 5, phố Hàng Bài, Hoàn Kiếm, Hà Nội",
                HeightCm = 140m,
                WeightKg = 35m,
                Bio = "Học sinh lớp 4, thích vẽ tranh."
            },
            new MemberProfile
            {
                MemberId = members[4].Id,
                NationalId = "001055000005",
                Nationality = "Việt Nam",
                Ethnicity = "Kinh",
                BloodType = "B+",
                MaritalStatus = MaritalStatus.Married,
                EducationLevel = EducationLevel.HighSchool,
                Occupation = "Hưu trí",
                BirthPlace = "Thái Bình",
                CurrentAddress = "Số 12, ngõ 5, phố Hàng Bài, Hoàn Kiếm, Hà Nội",
                HeightCm = 165m,
                WeightKg = 62m,
                EmergencyContactName = "Nguyễn Văn An",
                EmergencyContactPhone = "0901234567",
                Bio = "Ông nội đã nghỉ hưu, thích chăm sóc cây cảnh."
            }
        };

        context.MemberProfiles.AddRange(profiles);
        await context.SaveChangesAsync();
    }

    private static async Task SeedMemberRelationshipsAsync(ApplicationDbContext context)
    {
        if (await context.MemberRelationships.AnyAsync())
        {
            return;
        }

        var members = await context.FamilyMembers.OrderBy(m => m.DateOfBirth).ToListAsync();
        if (members.Count < 6) return;

        // members: [0]=An (chủ hộ), [1]=Bình (vợ), [2]=Cường (con), [3]=Dung (con), [4]=Em (ông), [5]=Hoa (bà)
        context.MemberRelationships.AddRange(
            new MemberRelationship
            {
                MemberId = members[0].Id, RelatedMemberId = members[1].Id,
                RelationshipType = RelationshipType.Spouse,
                StartedAt = new DateOnly(2008, 10, 10), IsBiological = false,
                Notes = "Vợ chồng."
            },
            new MemberRelationship
            {
                MemberId = members[0].Id, RelatedMemberId = members[2].Id,
                RelationshipType = RelationshipType.Child,
                IsBiological = true, Notes = "Con trai cả."
            },
            new MemberRelationship
            {
                MemberId = members[0].Id, RelatedMemberId = members[3].Id,
                RelationshipType = RelationshipType.Child,
                IsBiological = true, Notes = "Con gái út."
            },
            new MemberRelationship
            {
                MemberId = members[4].Id, RelatedMemberId = members[0].Id,
                RelationshipType = RelationshipType.Child,
                IsBiological = true, Notes = "An là con trai của ông Em."
            },
            new MemberRelationship
            {
                MemberId = members[4].Id, RelatedMemberId = members[5].Id,
                RelationshipType = RelationshipType.Spouse,
                StartedAt = new DateOnly(1978, 2, 1), IsBiological = false,
                Notes = "Ông bà nội."
            },
            new MemberRelationship
            {
                MemberId = members[2].Id, RelatedMemberId = members[3].Id,
                RelationshipType = RelationshipType.Sibling,
                IsBiological = true, Notes = "Anh em ruột."
            });

        await context.SaveChangesAsync();
    }

    // =====================================================================================
    // Documents
    // =====================================================================================
    private static async Task SeedDocumentFilesAsync(ApplicationDbContext context)
    {
        if (await context.Files.AnyAsync())
        {
            return;
        }

        var users = await context.Users.OrderBy(u => u.Email).ToListAsync();
        if (users.Count == 0) return;

        var files = new[]
        {
            new DocumentFile { OriginalName = "ho-khau.pdf", StoredName = "hk-2024.pdf", StoragePath = "/uploads/2024/ho-khau.pdf", PublicUrl = "/files/ho-khau.pdf", FileType = "pdf", MimeType = "application/pdf", SizeBytes = 245678 },
            new DocumentFile { OriginalName = "giay-khai-sinh-cuong.pdf", StoredName = "gks-cuong.pdf", StoragePath = "/uploads/2024/gks-cuong.pdf", PublicUrl = "/files/gks-cuong.pdf", FileType = "pdf", MimeType = "application/pdf", SizeBytes = 132045 },
            new DocumentFile { OriginalName = "anh-gia-dinh-tet.jpg", StoredName = "tet-2024.jpg", StoragePath = "/uploads/2024/tet-2024.jpg", PublicUrl = "/files/tet-2024.jpg", FileType = "jpg", MimeType = "image/jpeg", SizeBytes = 1893421 },
            new DocumentFile { OriginalName = "hop-dong-thue-nha.docx", StoredName = "hd-thue-nha.docx", StoragePath = "/uploads/2024/hd-thue-nha.docx", PublicUrl = "/files/hd-thue-nha.docx", FileType = "docx", MimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document", SizeBytes = 56120 },
            new DocumentFile { OriginalName = "ke-hoach-chi-tieu.xlsx", StoredName = "chi-tieu-2024.xlsx", StoragePath = "/uploads/2024/chi-tieu-2024.xlsx", PublicUrl = "/files/chi-tieu-2024.xlsx", FileType = "xlsx", MimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", SizeBytes = 38900 },
            new DocumentFile { OriginalName = "so-kham-benh.pdf", StoredName = "skb-an.pdf", StoragePath = "/uploads/2024/skb-an.pdf", PublicUrl = "/files/skb-an.pdf", FileType = "pdf", MimeType = "application/pdf", SizeBytes = 421890 }
        };

        for (var i = 0; i < files.Length; i++)
        {
            files[i].UploadedByUserId = users[i % users.Count].Id;
        }

        context.Files.AddRange(files);
        await context.SaveChangesAsync();
    }

    private static async Task SeedDocumentsAsync(ApplicationDbContext context)
    {
        if (await context.Documents.AnyAsync())
        {
            return;
        }

        var categories = await context.Categories.OrderBy(c => c.SortOrder).ToListAsync();
        var users = await context.Users.OrderBy(u => u.Email).ToListAsync();
        var members = await context.FamilyMembers.OrderBy(m => m.DateOfBirth).ToListAsync();
        var files = await context.Files.OrderBy(f => f.OriginalName).ToListAsync();
        if (categories.Count == 0 || users.Count == 0) return;

        var now = DateTime.UtcNow;
        var documents = new[]
        {
            new Document { Title = "Sổ hộ khẩu gia đình", Slug = "so-ho-khau-gia-dinh", Summary = "Bản scan sổ hộ khẩu mới nhất", ContentType = "file", Content = "Sổ hộ khẩu của hộ ông Nguyễn Văn An.", CategoryId = categories[3 % categories.Count].Id, CreatedByUserId = users[0].Id, IsPublished = true, PublishedAt = now.AddDays(-30), ViewCount = 12, SortOrder = 1 },
            new Document { Title = "Hướng dẫn sử dụng hệ thống quản lý gia đình", Slug = "huong-dan-su-dung-he-thong", Summary = "Tài liệu hướng dẫn các bước cơ bản", ContentType = "text", Content = "Bước 1: Đăng nhập. Bước 2: Thêm thành viên...", CategoryId = categories[1 % categories.Count].Id, CreatedByUserId = users[1 % users.Count].Id, IsPublished = true, PublishedAt = now.AddDays(-20), ViewCount = 45, SortOrder = 2 },
            new Document { Title = "Quy định chi tiêu chung của gia đình", Slug = "quy-dinh-chi-tieu-chung", Summary = "Chính sách quản lý ngân sách", ContentType = "text", Content = "Mỗi tháng trích 20% thu nhập vào quỹ dự phòng.", CategoryId = categories[2 % categories.Count].Id, CreatedByUserId = users[0].Id, IsPublished = true, PublishedAt = now.AddDays(-15), ViewCount = 8, SortOrder = 3 },
            new Document { Title = "Hợp đồng thuê nhà 2024", Slug = "hop-dong-thue-nha-2024", Summary = "Hợp đồng thuê căn hộ", ContentType = "file", Content = "Hợp đồng thuê nhà tại Hoàn Kiếm.", CategoryId = categories[3 % categories.Count].Id, CreatedByUserId = users[2 % users.Count].Id, IsPublished = false, ViewCount = 3, SortOrder = 4 },
            new Document { Title = "Album ảnh Tết Nguyên Đán", Slug = "album-anh-tet-nguyen-dan", Summary = "Hình ảnh kỷ niệm Tết", ContentType = "file", Content = "Album ảnh chụp Tết 2024.", CategoryId = categories[4 % categories.Count].Id, CreatedByUserId = users[1 % users.Count].Id, IsPublished = true, PublishedAt = now.AddDays(-5), ViewCount = 67, SortOrder = 5 },
            new Document { Title = "Sổ khám bệnh của ông nội", Slug = "so-kham-benh-ong-noi", Summary = "Hồ sơ y tế cá nhân", ContentType = "file", Content = "Sổ khám bệnh định kỳ tại bệnh viện Bạch Mai.", CategoryId = categories[5 % categories.Count].Id, CreatedByUserId = users[0].Id, IsPublished = false, ViewCount = 2, SortOrder = 6 }
        };

        // Gán file và thành viên liên quan khi có.
        for (var i = 0; i < documents.Length; i++)
        {
            if (files.Count > 0) documents[i].FileId = files[i % files.Count].Id;
            if (members.Count > 0 && (i == 0 || i == 5)) documents[i].MemberId = members[i % members.Count].Id;
        }

        context.Documents.AddRange(documents);
        await context.SaveChangesAsync();
    }

    private static async Task SeedDocumentTagsAsync(ApplicationDbContext context)
    {
        if (await context.DocumentTags.AnyAsync())
        {
            return;
        }

        var documents = await context.Documents.OrderBy(d => d.SortOrder).ToListAsync();
        var tags = await context.Tags.OrderBy(t => t.Slug).ToListAsync();
        if (documents.Count == 0 || tags.Count == 0) return;

        var now = DateTime.UtcNow;
        var joins = new[]
        {
            new DocumentTag { DocumentId = documents[0].Id, TagId = tags[0 % tags.Count].Id, AddedAt = now.AddDays(-30) },
            new DocumentTag { DocumentId = documents[0].Id, TagId = tags[1 % tags.Count].Id, AddedAt = now.AddDays(-30) },
            new DocumentTag { DocumentId = documents[1 % documents.Count].Id, TagId = tags[2 % tags.Count].Id, AddedAt = now.AddDays(-20) },
            new DocumentTag { DocumentId = documents[2 % documents.Count].Id, TagId = tags[0 % tags.Count].Id, AddedAt = now.AddDays(-15) },
            new DocumentTag { DocumentId = documents[3 % documents.Count].Id, TagId = tags[3 % tags.Count].Id, AddedAt = now.AddDays(-10) },
            new DocumentTag { DocumentId = documents[4 % documents.Count].Id, TagId = tags[1 % tags.Count].Id, AddedAt = now.AddDays(-5) }
        };

        context.DocumentTags.AddRange(joins);
        await context.SaveChangesAsync();
    }

    private static async Task SeedDocumentVersionsAsync(ApplicationDbContext context)
    {
        if (await context.DocumentVersions.AnyAsync())
        {
            return;
        }

        var documents = await context.Documents.OrderBy(d => d.SortOrder).ToListAsync();
        var users = await context.Users.OrderBy(u => u.Email).ToListAsync();
        if (documents.Count == 0 || users.Count == 0) return;

        var versions = new[]
        {
            new DocumentVersion { DocumentId = documents[0].Id, VersionNumber = 1, Title = documents[0].Title, Content = "Phiên bản gốc của sổ hộ khẩu.", ChangeSummary = "Tạo mới", EditedByUserId = users[0].Id },
            new DocumentVersion { DocumentId = documents[1 % documents.Count].Id, VersionNumber = 1, Title = documents[1 % documents.Count].Title, Content = "Bản nháp hướng dẫn.", ChangeSummary = "Tạo mới", EditedByUserId = users[1 % users.Count].Id },
            new DocumentVersion { DocumentId = documents[1 % documents.Count].Id, VersionNumber = 2, Title = documents[1 % documents.Count].Title, Content = "Bổ sung phần FAQ.", ChangeSummary = "Thêm mục câu hỏi thường gặp", EditedByUserId = users[0].Id },
            new DocumentVersion { DocumentId = documents[2 % documents.Count].Id, VersionNumber = 1, Title = documents[2 % documents.Count].Title, Content = "Quy định chi tiêu phiên bản 1.", ChangeSummary = "Tạo mới", EditedByUserId = users[2 % users.Count].Id },
            new DocumentVersion { DocumentId = documents[3 % documents.Count].Id, VersionNumber = 1, Title = documents[3 % documents.Count].Title, Content = "Hợp đồng bản đầu.", ChangeSummary = "Tạo mới", EditedByUserId = users[1 % users.Count].Id },
            new DocumentVersion { DocumentId = documents[4 % documents.Count].Id, VersionNumber = 1, Title = documents[4 % documents.Count].Title, Content = "Album ảnh phiên bản đầu.", ChangeSummary = "Tạo mới", EditedByUserId = users[0].Id }
        };

        context.DocumentVersions.AddRange(versions);
        await context.SaveChangesAsync();
    }

    // =====================================================================================
    // Finance
    // =====================================================================================
    private static async Task SeedAccountsAsync(ApplicationDbContext context)
    {
        if (await context.Accounts.AnyAsync())
        {
            return;
        }

        var members = await context.FamilyMembers.OrderBy(m => m.DateOfBirth).ToListAsync();

        var accounts = new[]
        {
            new Account { Name = "Tài khoản Vietcombank", AccountType = "bank", AccountNumber = "0123456789", BankName = "Vietcombank", Currency = "VND" },
            new Account { Name = "Tài khoản Techcombank", AccountType = "bank", AccountNumber = "1900123456", BankName = "Techcombank", Currency = "VND" },
            new Account { Name = "Ví tiền mặt", AccountType = "cash", Currency = "VND" },
            new Account { Name = "Thẻ tín dụng VPBank", AccountType = "credit", AccountNumber = "9704123456789012", BankName = "VPBank", Currency = "VND" },
            new Account { Name = "Sổ tiết kiệm BIDV", AccountType = "savings", AccountNumber = "5500987654", BankName = "BIDV", Currency = "VND" },
            new Account { Name = "Ví điện tử Momo", AccountType = "ewallet", AccountNumber = "0901234567", Currency = "VND" }
        };

        for (var i = 0; i < accounts.Length; i++)
        {
            if (members.Count > 0) accounts[i].MemberId = members[i % members.Count].Id;
        }

        context.Accounts.AddRange(accounts);
        await context.SaveChangesAsync();
    }

    private static async Task SeedRecurringTransactionsAsync(ApplicationDbContext context)
    {
        if (await context.RecurringTransactions.AnyAsync())
        {
            return;
        }

        var accounts = await context.Accounts.OrderBy(a => a.Name).ToListAsync();
        var categories = await context.Categories.OrderBy(c => c.SortOrder).ToListAsync();
        if (accounts.Count == 0) return;

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var recurrings = new[]
        {
            new RecurringTransaction { AccountId = accounts[0].Id, Name = "Lương tháng", Type = "income", Amount = 25000000m, Frequency = "monthly", StartDate = new DateOnly(2024, 1, 1), NextDueDate = today.AddDays(5) },
            new RecurringTransaction { AccountId = accounts[1 % accounts.Count].Id, Name = "Tiền thuê nhà", Type = "expense", Amount = 8000000m, Frequency = "monthly", StartDate = new DateOnly(2024, 1, 5), NextDueDate = today.AddDays(3) },
            new RecurringTransaction { AccountId = accounts[2 % accounts.Count].Id, Name = "Hóa đơn điện nước", Type = "expense", Amount = 1500000m, Frequency = "monthly", StartDate = new DateOnly(2024, 1, 10), NextDueDate = today.AddDays(8) },
            new RecurringTransaction { AccountId = accounts[3 % accounts.Count].Id, Name = "Internet và truyền hình", Type = "expense", Amount = 350000m, Frequency = "monthly", StartDate = new DateOnly(2024, 1, 15), NextDueDate = today.AddDays(12) },
            new RecurringTransaction { AccountId = accounts[4 % accounts.Count].Id, Name = "Học phí cho con", Type = "expense", Amount = 4000000m, Frequency = "monthly", StartDate = new DateOnly(2024, 9, 1), NextDueDate = today.AddDays(20) },
            new RecurringTransaction { AccountId = accounts[0].Id, Name = "Gửi tiết kiệm định kỳ", Type = "transfer", Amount = 5000000m, Frequency = "monthly", StartDate = new DateOnly(2024, 1, 25), NextDueDate = today.AddDays(25) }
        };

        for (var i = 0; i < recurrings.Length; i++)
        {
            if (categories.Count > 0) recurrings[i].CategoryId = categories[i % categories.Count].Id;
        }

        context.RecurringTransactions.AddRange(recurrings);
        await context.SaveChangesAsync();
    }

    private static async Task SeedTransactionsAsync(ApplicationDbContext context)
    {
        if (await context.Transactions.AnyAsync())
        {
            return;
        }

        var accounts = await context.Accounts.OrderBy(a => a.Name).ToListAsync();
        var categories = await context.Categories.OrderBy(c => c.SortOrder).ToListAsync();
        var members = await context.FamilyMembers.OrderBy(m => m.DateOfBirth).ToListAsync();
        if (accounts.Count == 0) return;

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var transactions = new[]
        {
            new Transaction { AccountId = accounts[0].Id, Type = "income", Amount = 25000000m, Description = "Lương tháng 5", TransactionDate = today.AddDays(-20), Status = TransactionStatus.Completed },
            new Transaction { AccountId = accounts[1 % accounts.Count].Id, Type = "expense", Amount = 8000000m, Description = "Thanh toán tiền thuê nhà", TransactionDate = today.AddDays(-18), Status = TransactionStatus.Completed },
            new Transaction { AccountId = accounts[2 % accounts.Count].Id, Type = "expense", Amount = 1250000m, Description = "Đi chợ cuối tuần", Note = "Thực phẩm tươi sống", TransactionDate = today.AddDays(-10), Status = TransactionStatus.Completed },
            new Transaction { AccountId = accounts[3 % accounts.Count].Id, Type = "expense", Amount = 2300000m, Description = "Mua sắm quần áo", TransactionDate = today.AddDays(-7), Status = TransactionStatus.Pending },
            new Transaction { AccountId = accounts[4 % accounts.Count].Id, Type = "income", Amount = 1200000m, Description = "Lãi tiết kiệm", TransactionDate = today.AddDays(-3), Status = TransactionStatus.Completed },
            new Transaction { AccountId = accounts[0].Id, Type = "expense", Amount = 4000000m, Description = "Đóng học phí cho con", TransactionDate = today.AddDays(-1), Status = TransactionStatus.Completed }
        };

        for (var i = 0; i < transactions.Length; i++)
        {
            if (categories.Count > 0) transactions[i].CategoryId = categories[i % categories.Count].Id;
            if (members.Count > 0) transactions[i].MemberId = members[i % members.Count].Id;
        }

        context.Transactions.AddRange(transactions);
        await context.SaveChangesAsync();
    }

    private static async Task SeedBudgetsAsync(ApplicationDbContext context)
    {
        if (await context.Budgets.AnyAsync())
        {
            return;
        }

        var categories = await context.Categories.OrderBy(c => c.SortOrder).ToListAsync();
        var members = await context.FamilyMembers.OrderBy(m => m.DateOfBirth).ToListAsync();

        var budgets = new[]
        {
            new Budget { Name = "Ngân sách ăn uống", Amount = 6000000m, PeriodType = "monthly", StartDate = new DateOnly(2024, 1, 1), EndDate = new DateOnly(2024, 12, 31), AlertThreshold = 0.8m },
            new Budget { Name = "Ngân sách đi lại", Amount = 2000000m, PeriodType = "monthly", StartDate = new DateOnly(2024, 1, 1), EndDate = new DateOnly(2024, 12, 31), AlertThreshold = 0.9m },
            new Budget { Name = "Ngân sách giáo dục", Amount = 5000000m, PeriodType = "monthly", StartDate = new DateOnly(2024, 9, 1), EndDate = new DateOnly(2025, 6, 30), AlertThreshold = 0.85m },
            new Budget { Name = "Ngân sách giải trí", Amount = 3000000m, PeriodType = "monthly", StartDate = new DateOnly(2024, 1, 1), EndDate = new DateOnly(2024, 12, 31), AlertThreshold = 0.75m },
            new Budget { Name = "Ngân sách y tế", Amount = 4000000m, PeriodType = "quarterly", StartDate = new DateOnly(2024, 1, 1), EndDate = new DateOnly(2024, 12, 31), AlertThreshold = 0.8m },
            new Budget { Name = "Ngân sách dự phòng", Amount = 10000000m, PeriodType = "yearly", StartDate = new DateOnly(2024, 1, 1), EndDate = new DateOnly(2024, 12, 31), AlertThreshold = 0.95m }
        };

        for (var i = 0; i < budgets.Length; i++)
        {
            if (categories.Count > 0) budgets[i].CategoryId = categories[i % categories.Count].Id;
            if (members.Count > 0) budgets[i].MemberId = members[0].Id;
        }

        context.Budgets.AddRange(budgets);
        await context.SaveChangesAsync();
    }

    private static async Task SeedInvestmentsAsync(ApplicationDbContext context)
    {
        if (await context.Investments.AnyAsync())
        {
            return;
        }

        var accounts = await context.Accounts.OrderBy(a => a.Name).ToListAsync();
        var members = await context.FamilyMembers.OrderBy(m => m.DateOfBirth).ToListAsync();

        var investments = new[]
        {
            new Investment { Type = "stock", Name = "Cổ phiếu FPT", Symbol = "FPT", Quantity = 500m, PurchasePrice = 95000m, CurrentPrice = 132000m },
            new Investment { Type = "stock", Name = "Cổ phiếu VNM", Symbol = "VNM", Quantity = 300m, PurchasePrice = 78000m, CurrentPrice = 71000m },
            new Investment { Type = "gold", Name = "Vàng SJC", Symbol = "SJC", Quantity = 5m, PurchasePrice = 75000000m, CurrentPrice = 84000000m },
            new Investment { Type = "fund", Name = "Quỹ mở VESAF", Symbol = "VESAF", Quantity = 1000m, PurchasePrice = 22000m, CurrentPrice = 25500m },
            new Investment { Type = "crypto", Name = "Bitcoin", Symbol = "BTC", Quantity = 0.05m, PurchasePrice = 1100000000m, CurrentPrice = 1500000000m },
            new Investment { Type = "bond", Name = "Trái phiếu chính phủ 5 năm", Symbol = "TPCP5Y", Quantity = 10m, PurchasePrice = 10000000m, CurrentPrice = 10300000m }
        };

        for (var i = 0; i < investments.Length; i++)
        {
            if (accounts.Count > 0) investments[i].AccountId = accounts[i % accounts.Count].Id;
            if (members.Count > 0) investments[i].MemberId = members[i % members.Count].Id;
        }

        context.Investments.AddRange(investments);
        await context.SaveChangesAsync();
    }

    // =====================================================================================
    // Medical
    // =====================================================================================
    private static async Task SeedMedicalRecordsAsync(ApplicationDbContext context)
    {
        if (await context.MedicalRecords.AnyAsync())
        {
            return;
        }

        var members = await context.FamilyMembers.OrderBy(m => m.DateOfBirth).ToListAsync();
        var users = await context.Users.OrderBy(u => u.Email).ToListAsync();
        if (members.Count == 0) return;

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var records = new[]
        {
            new MedicalRecord { MemberId = members[0].Id, RecordType = "checkup", Title = "Khám sức khỏe định kỳ", Diagnosis = "Sức khỏe tốt", Treatment = "Không cần điều trị", DoctorName = "BS. Trần Văn Hùng", HospitalName = "Bệnh viện Bạch Mai", RecordDate = today.AddDays(-60), FollowUpDate = today.AddDays(120), Notes = "Khuyến nghị tập thể dục đều đặn." },
            new MedicalRecord { MemberId = members[1 % members.Count].Id, RecordType = "illness", Title = "Cảm cúm mùa", Diagnosis = "Viêm họng cấp", Treatment = "Kháng sinh, nghỉ ngơi", DoctorName = "BS. Nguyễn Thị Lan", HospitalName = "Phòng khám Đa khoa Hà Nội", RecordDate = today.AddDays(-30), Notes = "Uống nhiều nước." },
            new MedicalRecord { MemberId = members[2 % members.Count].Id, RecordType = "vaccination", Title = "Tiêm vắc-xin cúm", Diagnosis = "Tiêm phòng định kỳ", Treatment = "Vắc-xin cúm mùa", DoctorName = "BS. Lê Văn Toàn", HospitalName = "Trung tâm Y tế dự phòng", RecordDate = today.AddDays(-90), Notes = "Không có phản ứng phụ." },
            new MedicalRecord { MemberId = members[4 % members.Count].Id, RecordType = "chronic", Title = "Theo dõi huyết áp", Diagnosis = "Tăng huyết áp độ 1", Treatment = "Thuốc hạ áp, ăn nhạt", DoctorName = "BS. Phạm Văn Đức", HospitalName = "Bệnh viện Lão khoa Trung ương", RecordDate = today.AddDays(-15), FollowUpDate = today.AddDays(15), IsPrivate = true, Notes = "Đo huyết áp hàng ngày." },
            new MedicalRecord { MemberId = members[3 % members.Count].Id, RecordType = "dental", Title = "Khám răng định kỳ", Diagnosis = "Sâu răng nhẹ", Treatment = "Trám răng", DoctorName = "BS. Vũ Thị Hương", HospitalName = "Nha khoa Quốc tế", RecordDate = today.AddDays(-45), Notes = "Hẹn tái khám sau 6 tháng." },
            new MedicalRecord { MemberId = members[0].Id, RecordType = "surgery", Title = "Tiểu phẫu ruột thừa", Diagnosis = "Viêm ruột thừa cấp", Treatment = "Phẫu thuật nội soi", DoctorName = "BS. Đỗ Văn Minh", HospitalName = "Bệnh viện Việt Đức", RecordDate = today.AddDays(-200), IsPrivate = true, Notes = "Hồi phục tốt." }
        };

        for (var i = 0; i < records.Length; i++)
        {
            if (users.Count > 0) records[i].CreatedByUserId = users[i % users.Count].Id;
        }

        context.MedicalRecords.AddRange(records);
        await context.SaveChangesAsync();
    }

    private static async Task SeedMedicationsAsync(ApplicationDbContext context)
    {
        if (await context.Medications.AnyAsync())
        {
            return;
        }

        var members = await context.FamilyMembers.OrderBy(m => m.DateOfBirth).ToListAsync();
        var records = await context.MedicalRecords.OrderBy(r => r.RecordDate).ToListAsync();
        if (members.Count == 0) return;

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var medications = new[]
        {
            new Medication { MemberId = members[4 % members.Count].Id, Name = "Amlodipine 5mg", Dosage = "1 viên", Frequency = "Mỗi sáng", StartDate = today.AddDays(-15), ReminderTimes = "[\"07:00\"]", Notes = "Thuốc hạ huyết áp." },
            new Medication { MemberId = members[1 % members.Count].Id, Name = "Paracetamol 500mg", Dosage = "1-2 viên", Frequency = "Khi sốt", StartDate = today.AddDays(-30), EndDate = today.AddDays(-25), IsActive = false, Notes = "Hạ sốt giảm đau." },
            new Medication { MemberId = members[0].Id, Name = "Vitamin tổng hợp", Dosage = "1 viên", Frequency = "Mỗi sáng", StartDate = today.AddDays(-60), ReminderTimes = "[\"08:00\"]", Notes = "Bổ sung dinh dưỡng." },
            new Medication { MemberId = members[2 % members.Count].Id, Name = "Vitamin D3", Dosage = "1 giọt", Frequency = "Mỗi ngày", StartDate = today.AddDays(-90), ReminderTimes = "[\"12:00\"]", Notes = "Bổ sung canxi cho trẻ." },
            new Medication { MemberId = members[3 % members.Count].Id, Name = "Siro ho thảo dược", Dosage = "10ml", Frequency = "3 lần/ngày", StartDate = today.AddDays(-10), EndDate = today.AddDays(-3), IsActive = false, ReminderTimes = "[\"08:00\",\"13:00\",\"20:00\"]", Notes = "Giảm ho." },
            new Medication { MemberId = members[4 % members.Count].Id, Name = "Aspirin 81mg", Dosage = "1 viên", Frequency = "Mỗi tối", StartDate = today.AddDays(-15), ReminderTimes = "[\"21:00\"]", Notes = "Phòng ngừa tim mạch." }
        };

        for (var i = 0; i < medications.Length; i++)
        {
            if (records.Count > 0 && (i == 0 || i == 5)) medications[i].MedicalRecordId = records[i % records.Count].Id;
        }

        context.Medications.AddRange(medications);
        await context.SaveChangesAsync();
    }

    private static async Task SeedHealthMetricsAsync(ApplicationDbContext context)
    {
        if (await context.HealthMetrics.AnyAsync())
        {
            return;
        }

        var members = await context.FamilyMembers.OrderBy(m => m.DateOfBirth).ToListAsync();
        if (members.Count == 0) return;

        var now = DateTime.UtcNow;
        var metrics = new[]
        {
            new HealthMetric { MemberId = members[4 % members.Count].Id, MetricType = "blood_pressure_systolic", Value = 138m, Unit = "mmHg", MeasuredAt = now.AddDays(-1), Notes = "Đo buổi sáng." },
            new HealthMetric { MemberId = members[4 % members.Count].Id, MetricType = "blood_pressure_diastolic", Value = 88m, Unit = "mmHg", MeasuredAt = now.AddDays(-1), Notes = "Đo buổi sáng." },
            new HealthMetric { MemberId = members[0].Id, MetricType = "weight", Value = 68m, Unit = "kg", MeasuredAt = now.AddDays(-7) },
            new HealthMetric { MemberId = members[1 % members.Count].Id, MetricType = "weight", Value = 54m, Unit = "kg", MeasuredAt = now.AddDays(-7) },
            new HealthMetric { MemberId = members[2 % members.Count].Id, MetricType = "height", Value = 158m, Unit = "cm", MeasuredAt = now.AddDays(-30), Notes = "Đo định kỳ tháng." },
            new HealthMetric { MemberId = members[0].Id, MetricType = "heart_rate", Value = 72m, Unit = "bpm", MeasuredAt = now.AddDays(-2) }
        };

        context.HealthMetrics.AddRange(metrics);
        await context.SaveChangesAsync();
    }

    // =====================================================================================
    // Education
    // =====================================================================================
    private static async Task SeedEducationRecordsAsync(ApplicationDbContext context)
    {
        if (await context.EducationRecords.AnyAsync())
        {
            return;
        }

        var members = await context.FamilyMembers.OrderBy(m => m.DateOfBirth).ToListAsync();
        if (members.Count == 0) return;

        var records = new[]
        {
            new EducationRecord { MemberId = members[2 % members.Count].Id, InstitutionName = "Trường THCS Trưng Vương", Level = "Trung học cơ sở", Major = "Phổ thông", StudentId = "TV2023001", StartDate = new DateOnly(2021, 9, 5), Gpa = 8.5m, Status = EducationStatus.Enrolled, Achievements = "[\"Học sinh giỏi 2 năm liền.\"]" },
            new EducationRecord { MemberId = members[3 % members.Count].Id, InstitutionName = "Trường Tiểu học Kim Liên", Level = "Tiểu học", Major = "Phổ thông", StudentId = "KL2022045", StartDate = new DateOnly(2020, 9, 5), Gpa = 9.2m, Status = EducationStatus.Enrolled, Achievements = "[\"Đạt giải vẽ tranh cấp trường.\"]" },
            new EducationRecord { MemberId = members[0].Id, InstitutionName = "Đại học Xây dựng Hà Nội", Level = "Đại học", Major = "Kỹ thuật xây dựng", Degree = "Kỹ sư", StudentId = "XD2002112", StartDate = new DateOnly(1998, 9, 1), EndDate = new DateOnly(2003, 6, 30), Gpa = 3.4m, Status = EducationStatus.Graduated, Achievements = "[\"Tốt nghiệp loại Khá.\"]" },
            new EducationRecord { MemberId = members[1 % members.Count].Id, InstitutionName = "Đại học Sư phạm Hà Nội", Level = "Đại học", Major = "Giáo dục tiểu học", Degree = "Cử nhân", StudentId = "SP2001078", StartDate = new DateOnly(2001, 9, 1), EndDate = new DateOnly(2005, 6, 30), Gpa = 3.6m, Status = EducationStatus.Graduated, Achievements = "[\"Tốt nghiệp loại Giỏi.\"]" },
            new EducationRecord { MemberId = members[1 % members.Count].Id, InstitutionName = "Trung tâm Anh ngữ ILA", Level = "Chứng chỉ", Major = "Tiếng Anh giao tiếp", StartDate = new DateOnly(2023, 3, 1), Status = EducationStatus.OnLeave, Notes = "Tạm hoãn do bận công việc." },
            new EducationRecord { MemberId = members[2 % members.Count].Id, InstitutionName = "Câu lạc bộ Toán học Trẻ", Level = "Ngoại khóa", Major = "Toán nâng cao", StartDate = new DateOnly(2022, 10, 1), Status = EducationStatus.Enrolled, Achievements = "[\"Giải Ba cấp quận.\"]" }
        };

        context.EducationRecords.AddRange(records);
        await context.SaveChangesAsync();
    }

    private static async Task SeedStudySchedulesAsync(ApplicationDbContext context)
    {
        if (await context.StudySchedules.AnyAsync())
        {
            return;
        }

        var members = await context.FamilyMembers.OrderBy(m => m.DateOfBirth).ToListAsync();
        var records = await context.EducationRecords.OrderBy(r => r.InstitutionName).ToListAsync();
        if (members.Count == 0) return;

        var baseDay = DateTime.SpecifyKind(DateTime.UtcNow.Date.AddDays(1), DateTimeKind.Utc);
        var schedules = new[]
        {
            new StudySchedule { MemberId = members[2 % members.Count].Id, Title = "Học Toán nâng cao", Subject = "Toán", StartTime = baseDay.AddHours(8), EndTime = baseDay.AddHours(10), Location = "Phòng học CLB", TeacherName = "Thầy Hoàng", IsOnline = false, Status = StudyScheduleStatus.Scheduled },
            new StudySchedule { MemberId = members[2 % members.Count].Id, Title = "Luyện thi Tiếng Anh", Subject = "Tiếng Anh", StartTime = baseDay.AddDays(1).AddHours(14), EndTime = baseDay.AddDays(1).AddHours(16), IsOnline = true, MeetingUrl = "https://meet.example.com/eng-class", TeacherName = "Cô Mai", Status = StudyScheduleStatus.Scheduled },
            new StudySchedule { MemberId = members[3 % members.Count].Id, Title = "Lớp vẽ cuối tuần", Subject = "Mỹ thuật", StartTime = baseDay.AddDays(2).AddHours(9), EndTime = baseDay.AddDays(2).AddHours(11), Location = "Nhà văn hóa thiếu nhi", TeacherName = "Cô Lan", IsOnline = false, Status = StudyScheduleStatus.Scheduled },
            new StudySchedule { MemberId = members[3 % members.Count].Id, Title = "Ôn tập chính tả", Subject = "Tiếng Việt", StartTime = baseDay.AddDays(-3).AddHours(19), EndTime = baseDay.AddDays(-3).AddHours(20), Location = "Tại nhà", IsOnline = false, Status = StudyScheduleStatus.Completed },
            new StudySchedule { MemberId = members[2 % members.Count].Id, Title = "Học nhóm môn Lý", Subject = "Vật lý", StartTime = baseDay.AddDays(3).AddHours(15), EndTime = baseDay.AddDays(3).AddHours(17), Location = "Thư viện", IsOnline = false, Status = StudyScheduleStatus.Scheduled },
            new StudySchedule { MemberId = members[1 % members.Count].Id, Title = "Khóa học tiếng Anh online", Subject = "Tiếng Anh", StartTime = baseDay.AddDays(-1).AddHours(20), EndTime = baseDay.AddDays(-1).AddHours(21), IsOnline = true, MeetingUrl = "https://meet.example.com/ila", Status = StudyScheduleStatus.Cancelled }
        };

        for (var i = 0; i < schedules.Length; i++)
        {
            if (records.Count > 0 && i < 3) schedules[i].EducationRecordId = records[i % records.Count].Id;
        }

        context.StudySchedules.AddRange(schedules);
        await context.SaveChangesAsync();
    }

    // =====================================================================================
    // Events
    // =====================================================================================
    private static async Task SeedFamilyEventsAsync(ApplicationDbContext context)
    {
        if (await context.FamilyEvents.AnyAsync())
        {
            return;
        }

        var users = await context.Users.OrderBy(u => u.Email).ToListAsync();
        var files = await context.Files.OrderBy(f => f.OriginalName).ToListAsync();
        if (users.Count == 0) return;

        var now = DateTime.UtcNow;
        var events = new[]
        {
            new FamilyEvent { Title = "Tết Nguyên Đán 2025", Description = "Sum họp gia đình đón Tết", EventType = "holiday", StartAt = now.AddDays(-100), EndAt = now.AddDays(-93), AllDay = true, Location = "Nhà ông bà nội", Status = EventStatus.Completed },
            new FamilyEvent { Title = "Sinh nhật bé Dung", Description = "Tổ chức sinh nhật lần thứ 11", EventType = "birthday", StartAt = now.AddDays(15).Date.AddHours(18), EndAt = now.AddDays(15).Date.AddHours(21), Location = "Nhà hàng Sen Hà Thành", Status = EventStatus.Planned },
            new FamilyEvent { Title = "Họp mặt gia đình hàng tháng", Description = "Bữa cơm gia đình cuối tháng", EventType = "gathering", StartAt = now.AddDays(7).Date.AddHours(11), EndAt = now.AddDays(7).Date.AddHours(14), Location = "Nhà chủ hộ", RecurrenceRule = "FREQ=MONTHLY;BYMONTHDAY=-1", Status = EventStatus.Planned },
            new FamilyEvent { Title = "Đi du lịch Đà Nẵng", Description = "Chuyến nghỉ hè của cả nhà", EventType = "trip", StartAt = now.AddDays(40).Date.AddHours(6), EndAt = now.AddDays(44).Date.AddHours(22), Location = "Đà Nẵng", Status = EventStatus.Planned },
            new FamilyEvent { Title = "Giỗ tổ tiên", Description = "Lễ giỗ tổ theo phong tục", EventType = "anniversary", StartAt = now.AddDays(-10).Date.AddHours(9), EndAt = now.AddDays(-10).Date.AddHours(13), AllDay = false, Location = "Nhà thờ họ", Status = EventStatus.Completed },
            new FamilyEvent { Title = "Khám sức khỏe tổng quát cả nhà", Description = "Lịch khám định kỳ năm", EventType = "appointment", StartAt = now.AddDays(25).Date.AddHours(7), EndAt = now.AddDays(25).Date.AddHours(12), Location = "Bệnh viện Bạch Mai", Status = EventStatus.Planned }
        };

        for (var i = 0; i < events.Length; i++)
        {
            events[i].CreatedByUserId = users[i % users.Count].Id;
            if (files.Count > 0 && (i == 0 || i == 4)) events[i].CoverFileId = files[i % files.Count].Id;
        }

        context.FamilyEvents.AddRange(events);
        await context.SaveChangesAsync();
    }

    private static async Task SeedEventMediaAsync(ApplicationDbContext context)
    {
        if (await context.EventMedia.AnyAsync())
        {
            return;
        }

        var events = await context.FamilyEvents.OrderBy(e => e.StartAt).ToListAsync();
        var files = await context.Files.OrderBy(f => f.OriginalName).ToListAsync();
        var users = await context.Users.OrderBy(u => u.Email).ToListAsync();
        if (events.Count == 0 || files.Count == 0) return;

        var media = new[]
        {
            new EventMedia { EventId = events[0].Id, FileId = files[0].Id, Caption = "Ảnh gia đình ngày mùng 1 Tết", SortOrder = 1 },
            new EventMedia { EventId = events[0].Id, FileId = files[1 % files.Count].Id, Caption = "Mâm cỗ Tết", SortOrder = 2 },
            new EventMedia { EventId = events[1 % events.Count].Id, FileId = files[2 % files.Count].Id, Caption = "Ảnh kỷ niệm sinh nhật", SortOrder = 1 },
            new EventMedia { EventId = events[2 % events.Count].Id, FileId = files[3 % files.Count].Id, Caption = "Bữa cơm họp mặt", SortOrder = 1 },
            new EventMedia { EventId = events[3 % events.Count].Id, FileId = files[4 % files.Count].Id, Caption = "Ảnh chuẩn bị hành lý", SortOrder = 1 },
            new EventMedia { EventId = events[4 % events.Count].Id, FileId = files[5 % files.Count].Id, Caption = "Ảnh lễ giỗ", SortOrder = 1 }
        };

        for (var i = 0; i < media.Length; i++)
        {
            if (users.Count > 0) media[i].UploadedByUserId = users[i % users.Count].Id;
        }

        context.EventMedia.AddRange(media);
        await context.SaveChangesAsync();
    }

    // =====================================================================================
    // Assets
    // =====================================================================================
    private static async Task SeedAssetsAsync(ApplicationDbContext context)
    {
        if (await context.Assets.AnyAsync())
        {
            return;
        }

        var members = await context.FamilyMembers.OrderBy(m => m.DateOfBirth).ToListAsync();
        var categories = await context.Categories.OrderBy(c => c.SortOrder).ToListAsync();
        var users = await context.Users.OrderBy(u => u.Email).ToListAsync();

        var assets = new[]
        {
            new Asset { Name = "Căn hộ chung cư Times City", Description = "Căn hộ 2 phòng ngủ", AssetType = "real_estate", PurchasePrice = 3500000000m, PurchaseDate = new DateOnly(2018, 6, 15), Currency = "VND", Location = "Hai Bà Trưng, Hà Nội", Status = AssetStatus.Active, IsInsured = true, InsuranceInfo = "{\"provider\":\"Bảo Việt\",\"type\":\"cháy nổ\"}" },
            new Asset { Name = "Xe ô tô Toyota Vios", Description = "Xe gia đình 5 chỗ", AssetType = "vehicle", PurchasePrice = 550000000m, PurchaseDate = new DateOnly(2020, 3, 10), Currency = "VND", Location = "Hà Nội", SerialNumber = "30A-123.45", Status = AssetStatus.Active, IsInsured = true, InsuranceInfo = "{\"provider\":\"PVI\",\"type\":\"TNDS\"}" },
            new Asset { Name = "Xe máy Honda SH", Description = "Xe máy đi lại hàng ngày", AssetType = "vehicle", PurchasePrice = 95000000m, PurchaseDate = new DateOnly(2021, 8, 1), Currency = "VND", Location = "Hà Nội", SerialNumber = "29B1-678.90", Status = AssetStatus.Active, IsInsured = false },
            new Asset { Name = "Máy tính xách tay Dell XPS", Description = "Laptop làm việc", AssetType = "electronics", PurchasePrice = 32000000m, PurchaseDate = new DateOnly(2023, 1, 20), Currency = "VND", Location = "Tại nhà", SerialNumber = "DXPS-2023-001", Status = AssetStatus.Active, IsInsured = false },
            new Asset { Name = "Bộ trang sức vàng cưới", Description = "Vàng cưới gia truyền", AssetType = "jewelry", PurchasePrice = 120000000m, PurchaseDate = new DateOnly(2008, 10, 10), Currency = "VND", Location = "Két sắt", Status = AssetStatus.Active, IsInsured = false },
            new Asset { Name = "Đàn piano Yamaha", Description = "Đàn piano cơ cho con học", AssetType = "furniture", PurchasePrice = 65000000m, PurchaseDate = new DateOnly(2022, 5, 5), Currency = "VND", Location = "Phòng khách", Status = AssetStatus.Active, IsInsured = false }
        };

        for (var i = 0; i < assets.Length; i++)
        {
            if (members.Count > 0) assets[i].MemberId = members[i % members.Count].Id;
            if (categories.Count > 0) assets[i].CategoryId = categories[i % categories.Count].Id;
            if (users.Count > 0) assets[i].CreatedByUserId = users[i % users.Count].Id;
        }

        context.Assets.AddRange(assets);
        await context.SaveChangesAsync();
    }

    private static async Task SeedAssetValuationsAsync(ApplicationDbContext context)
    {
        if (await context.AssetValuations.AnyAsync())
        {
            return;
        }

        var assets = await context.Assets.OrderBy(a => a.Name).ToListAsync();
        var users = await context.Users.OrderBy(u => u.Email).ToListAsync();
        if (assets.Count == 0) return;

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var valuations = new[]
        {
            new AssetValuation { AssetId = assets[0].Id, ValuationDate = today.AddDays(-365), Value = 4200000000m, Currency = "VND", ValuationMethod = "market", Notes = "Định giá theo giá thị trường năm ngoái." },
            new AssetValuation { AssetId = assets[0].Id, ValuationDate = today.AddDays(-30), Value = 4500000000m, Currency = "VND", ValuationMethod = "market", Notes = "Định giá cập nhật gần nhất." },
            new AssetValuation { AssetId = assets[1 % assets.Count].Id, ValuationDate = today.AddDays(-60), Value = 420000000m, Currency = "VND", ValuationMethod = "depreciation", Notes = "Đã khấu hao sau 4 năm." },
            new AssetValuation { AssetId = assets[2 % assets.Count].Id, ValuationDate = today.AddDays(-90), Value = 70000000m, Currency = "VND", ValuationMethod = "depreciation" },
            new AssetValuation { AssetId = assets[3 % assets.Count].Id, ValuationDate = today.AddDays(-45), Value = 22000000m, Currency = "VND", ValuationMethod = "depreciation" },
            new AssetValuation { AssetId = assets[4 % assets.Count].Id, ValuationDate = today.AddDays(-15), Value = 150000000m, Currency = "VND", ValuationMethod = "appraisal", Notes = "Định giá theo giá vàng hiện tại." }
        };

        for (var i = 0; i < valuations.Length; i++)
        {
            if (users.Count > 0) valuations[i].CreatedByUserId = users[i % users.Count].Id;
        }

        context.AssetValuations.AddRange(valuations);
        await context.SaveChangesAsync();
    }

    // =====================================================================================
    // Utility
    // =====================================================================================
    private static async Task SeedRemindersAsync(ApplicationDbContext context)
    {
        if (await context.Reminders.AnyAsync())
        {
            return;
        }

        var users = await context.Users.OrderBy(u => u.Email).ToListAsync();
        var members = await context.FamilyMembers.OrderBy(m => m.DateOfBirth).ToListAsync();
        if (users.Count == 0) return;

        var now = DateTime.UtcNow;
        var reminders = new[]
        {
            new Reminder { UserId = users[0].Id, Title = "Đóng tiền điện nước", Description = "Hạn cuối ngày 10 hàng tháng", RemindAt = now.AddDays(3), RecurrenceRule = "FREQ=MONTHLY;BYMONTHDAY=8", Status = ReminderStatus.Pending },
            new Reminder { UserId = users[0].Id, Title = "Tái khám huyết áp cho ông nội", Description = "Mang theo sổ khám bệnh", RemindAt = now.AddDays(15), EntityType = "MedicalRecord", Status = ReminderStatus.Pending },
            new Reminder { UserId = users[1 % users.Count].Id, Title = "Họp phụ huynh cho bé Dung", Description = "Tại trường tiểu học Kim Liên", RemindAt = now.AddDays(7), Status = ReminderStatus.Pending },
            new Reminder { UserId = users[2 % users.Count].Id, Title = "Gia hạn bảo hiểm xe ô tô", Description = "Bảo hiểm TNDS sắp hết hạn", RemindAt = now.AddDays(20), EntityType = "Asset", Status = ReminderStatus.Pending },
            new Reminder { UserId = users[0].Id, Title = "Sinh nhật bé Dung", Description = "Chuẩn bị quà và đặt bàn", RemindAt = now.AddDays(14), RecurrenceRule = "FREQ=YEARLY", Status = ReminderStatus.Snoozed, SnoozedUntil = now.AddDays(1) },
            new Reminder { UserId = users[1 % users.Count].Id, Title = "Uống thuốc buổi tối", Description = "Nhắc ông nội uống Aspirin", RemindAt = now.AddHours(8), RecurrenceRule = "FREQ=DAILY", Status = ReminderStatus.Sent }
        };

        for (var i = 0; i < reminders.Length; i++)
        {
            if (members.Count > 0 && (i == 1 || i == 5)) reminders[i].MemberId = members[4 % members.Count].Id;
        }

        context.Reminders.AddRange(reminders);
        await context.SaveChangesAsync();
    }

    private static async Task SeedBookmarksAsync(ApplicationDbContext context)
    {
        if (await context.Bookmarks.AnyAsync())
        {
            return;
        }

        var users = await context.Users.OrderBy(u => u.Email).ToListAsync();
        var documents = await context.Documents.OrderBy(d => d.SortOrder).ToListAsync();
        var assets = await context.Assets.OrderBy(a => a.Name).ToListAsync();
        var events = await context.FamilyEvents.OrderBy(e => e.StartAt).ToListAsync();
        if (users.Count == 0) return;

        var bookmarks = new List<Bookmark>();

        if (documents.Count > 0)
        {
            bookmarks.Add(new Bookmark { UserId = users[0].Id, EntityType = "Document", EntityId = documents[0].Id, Note = "Tài liệu hộ khẩu quan trọng." });
            bookmarks.Add(new Bookmark { UserId = users[0].Id, EntityType = "Document", EntityId = documents[1 % documents.Count].Id, Note = "Đọc lại hướng dẫn." });
        }
        if (assets.Count > 0)
        {
            bookmarks.Add(new Bookmark { UserId = users[1 % users.Count].Id, EntityType = "Asset", EntityId = assets[0].Id, Note = "Theo dõi giá trị căn hộ." });
            bookmarks.Add(new Bookmark { UserId = users[2 % users.Count].Id, EntityType = "Asset", EntityId = assets[1 % assets.Count].Id });
        }
        if (events.Count > 0)
        {
            bookmarks.Add(new Bookmark { UserId = users[1 % users.Count].Id, EntityType = "FamilyEvent", EntityId = events[1 % events.Count].Id, Note = "Nhớ chuẩn bị cho chuyến đi." });
            bookmarks.Add(new Bookmark { UserId = users[0].Id, EntityType = "FamilyEvent", EntityId = events[0].Id });
        }

        // Fallback an toàn nếu thiếu dữ liệu phụ thuộc: vẫn đảm bảo >= 5 bản ghi.
        while (bookmarks.Count < 5)
        {
            bookmarks.Add(new Bookmark { UserId = users[bookmarks.Count % users.Count].Id, EntityType = "Note", EntityId = Guid.NewGuid(), Note = "Ghi chú nhanh." });
        }

        context.Bookmarks.AddRange(bookmarks);
        await context.SaveChangesAsync();
    }

    private static async Task SeedViewHistoriesAsync(ApplicationDbContext context)
    {
        if (await context.ViewHistories.AnyAsync())
        {
            return;
        }

        var users = await context.Users.OrderBy(u => u.Email).ToListAsync();
        var documents = await context.Documents.OrderBy(d => d.SortOrder).ToListAsync();
        if (users.Count == 0) return;

        var now = DateTime.UtcNow;
        var histories = new List<ViewHistory>();

        if (documents.Count > 0)
        {
            for (var i = 0; i < documents.Count && histories.Count < 6; i++)
            {
                histories.Add(new ViewHistory
                {
                    UserId = users[i % users.Count].Id,
                    EntityType = "Document",
                    EntityId = documents[i].Id,
                    ViewedAt = now.AddHours(-(i + 1) * 6),
                    DurationSeconds = 30 + i * 25
                });
            }
        }

        while (histories.Count < 5)
        {
            histories.Add(new ViewHistory
            {
                UserId = users[histories.Count % users.Count].Id,
                EntityType = "Dashboard",
                EntityId = Guid.NewGuid(),
                ViewedAt = now.AddHours(-histories.Count),
                DurationSeconds = 45
            });
        }

        context.ViewHistories.AddRange(histories);
        await context.SaveChangesAsync();
    }

    // =====================================================================================
    // System
    // =====================================================================================
    private static async Task SeedNotificationsAsync(ApplicationDbContext context)
    {
        if (await context.Notifications.AnyAsync())
        {
            return;
        }

        var users = await context.Users.OrderBy(u => u.Email).ToListAsync();
        if (users.Count == 0) return;

        var now = DateTime.UtcNow;
        var notifications = new[]
        {
            new Notification { UserId = users[0].Id, Title = "Chào mừng đến với hệ thống", Body = "Tài khoản của bạn đã được kích hoạt.", Type = "system", Channel = "in_app", IsRead = true, ReadAt = now.AddDays(-29), SentAt = now.AddDays(-30) },
            new Notification { UserId = users[0].Id, Title = "Nhắc nhở đóng tiền điện nước", Body = "Hạn cuối còn 3 ngày.", Type = "reminder", Channel = "in_app", EntityType = "Reminder", SentAt = now.AddHours(-2) },
            new Notification { UserId = users[1 % users.Count].Id, Title = "Có tài liệu mới được chia sẻ", Body = "Tài liệu 'Hướng dẫn sử dụng hệ thống' đã được cập nhật.", Type = "document", Channel = "email", EntityType = "Document", SentAt = now.AddDays(-1) },
            new Notification { UserId = users[2 % users.Count].Id, Title = "Cảnh báo ngân sách", Body = "Ngân sách ăn uống đã dùng 80%.", Type = "finance", Channel = "in_app", EntityType = "Budget", SentAt = now.AddHours(-12) },
            new Notification { UserId = users[1 % users.Count].Id, Title = "Sự kiện sắp tới", Body = "Sinh nhật bé Dung sau 15 ngày.", Type = "event", Channel = "push", EntityType = "FamilyEvent", SentAt = now.AddHours(-6) },
            new Notification { UserId = users[0].Id, Title = "Sao lưu dữ liệu thành công", Body = "Bản sao lưu hàng tuần đã hoàn tất.", Type = "system", Channel = "in_app", IsRead = true, ReadAt = now.AddDays(-1), SentAt = now.AddDays(-2) }
        };

        context.Notifications.AddRange(notifications);
        await context.SaveChangesAsync();
    }

    private static async Task SeedAuditLogsAsync(ApplicationDbContext context)
    {
        if (await context.AuditLogs.AnyAsync())
        {
            return;
        }

        var users = await context.Users.OrderBy(u => u.Email).ToListAsync();

        var logs = new[]
        {
            new AuditLog { Action = AuditAction.Login, EntityType = "User", NewValues = "{\"method\":\"password\"}" },
            new AuditLog { Action = AuditAction.Create, EntityType = "Document", EntityId = Guid.NewGuid(), NewValues = "{\"title\":\"Sổ hộ khẩu gia đình\"}" },
            new AuditLog { Action = AuditAction.Update, EntityType = "FamilyMember", EntityId = Guid.NewGuid(), OldValues = "{\"phone\":\"0900000000\"}", NewValues = "{\"phone\":\"0901234567\"}" },
            new AuditLog { Action = AuditAction.Delete, EntityType = "Transaction", EntityId = Guid.NewGuid(), OldValues = "{\"amount\":100000}" },
            new AuditLog { Action = AuditAction.Export, EntityType = "Report", NewValues = "{\"format\":\"pdf\"}" },
            new AuditLog { Action = AuditAction.Logout, EntityType = "User" }
        };

        for (var i = 0; i < logs.Length; i++)
        {
            if (users.Count > 0) logs[i].UserId = users[i % users.Count].Id;
        }

        context.AuditLogs.AddRange(logs);
        await context.SaveChangesAsync();
    }

    private static async Task SeedSystemConfigsAsync(ApplicationDbContext context)
    {
        if (await context.SystemConfigs.AnyAsync())
        {
            return;
        }

        context.SystemConfigs.AddRange(
            new SystemConfig { Key = "app.name", Value = "\"Hệ thống Quản lý Gia đình\"", Description = "Tên ứng dụng hiển thị", IsPublic = true },
            new SystemConfig { Key = "app.default_currency", Value = "\"VND\"", Description = "Đơn vị tiền tệ mặc định", IsPublic = true },
            new SystemConfig { Key = "app.timezone", Value = "\"Asia/Ho_Chi_Minh\"", Description = "Múi giờ mặc định", IsPublic = true },
            new SystemConfig { Key = "backup.schedule", Value = "{\"frequency\":\"weekly\",\"day\":\"sunday\",\"hour\":2}", Description = "Lịch sao lưu tự động", IsPublic = false },
            new SystemConfig { Key = "notification.channels", Value = "[\"in_app\",\"email\",\"push\"]", Description = "Các kênh thông báo được bật", IsPublic = false },
            new SystemConfig { Key = "security.password_policy", Value = "{\"minLength\":8,\"requireUppercase\":true,\"requireNumber\":true}", Description = "Chính sách mật khẩu", IsPublic = false });

        await context.SaveChangesAsync();
    }

    private static async Task SeedBackupLogsAsync(ApplicationDbContext context)
    {
        if (await context.BackupLogs.AnyAsync())
        {
            return;
        }

        var now = DateTime.UtcNow;

        // BackupLog không map CreatedAt; chỉ set các trường nghiệp vụ (StartedAt/CompletedAt...).
        context.BackupLogs.AddRange(
            new BackupLog { BackupType = "full", Status = "completed", FilePath = "/backups/full-2024-05-01.tar.gz", FileSize = 524288000, Checksum = "a1b2c3d4e5f6", StartedAt = now.AddDays(-28), CompletedAt = now.AddDays(-28).AddMinutes(12) },
            new BackupLog { BackupType = "incremental", Status = "completed", FilePath = "/backups/inc-2024-05-08.tar.gz", FileSize = 41943040, Checksum = "b2c3d4e5f6a1", StartedAt = now.AddDays(-21), CompletedAt = now.AddDays(-21).AddMinutes(3) },
            new BackupLog { BackupType = "incremental", Status = "completed", FilePath = "/backups/inc-2024-05-15.tar.gz", FileSize = 52428800, Checksum = "c3d4e5f6a1b2", StartedAt = now.AddDays(-14), CompletedAt = now.AddDays(-14).AddMinutes(4) },
            new BackupLog { BackupType = "full", Status = "failed", StartedAt = now.AddDays(-7), CompletedAt = now.AddDays(-7).AddMinutes(2), ErrorMessage = "Không đủ dung lượng đĩa." },
            new BackupLog { BackupType = "incremental", Status = "completed", FilePath = "/backups/inc-2024-05-22.tar.gz", FileSize = 47185920, Checksum = "d4e5f6a1b2c3", StartedAt = now.AddDays(-1), CompletedAt = now.AddDays(-1).AddMinutes(3) },
            new BackupLog { BackupType = "full", Status = "running", StartedAt = now.AddMinutes(-5) });

        await context.SaveChangesAsync();
    }
}
