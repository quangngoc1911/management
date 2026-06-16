using ManagementSystem.Application;
using ManagementSystem.Infrastructure;
using ManagementSystem.Application.Mappings;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Mvc;

using Scalar.AspNetCore;

using Serilog;

using Hellang.Middleware.ProblemDetails;

// Cho phép ghi DateTime (Kind=Unspecified/Local) vào cột 'timestamp with time zone' của PostgreSQL.
// Mặc định Npgsql chỉ chấp nhận UTC; bật legacy behavior để DateTime do client gửi lên được xử lý như UTC.
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Khởi tạo logger ngay từ đầu để bắt được cả lỗi khởi động hệ thống
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7) // Tối ưu RAM/Disk: chỉ giữ log 7 ngày gần nhất
    .CreateLogger();

try
{
    Log.Information("Starting web host");
    var builder = WebApplication.CreateBuilder(args);

    // Thay thế logger mặc định bằng Serilog
    builder.Host.UseSerilog();

    // --- Cấu hình lại đường dẫn appsettings ---
    builder.Configuration.Sources.Clear(); // Xóa các đường dẫn mặc định ở root

    builder.Configuration
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("Properties/appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"Properties/appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables(); // Giữ lại cái này để đọc các biến môi trường (như JWT_SECRET_KEY)
    // ------------------------------------------

    // Hiển thị môi trường đang chạy (Development, Staging, hoặc Production)
    Console.WriteLine($"Môi trường hiện tại: {builder.Environment.EnvironmentName}");
    Console.WriteLine($"Database Connection: {builder.Configuration.GetConnectionString("DefaultConnection")?.Substring(0, 10)}...");

    // ==========================================================================
    // 1. CẤU HÌNH DỊCH VỤ (DEPENDENCY INJECTION - DI)
    // ==========================================================================

    // Cấu hình Controllers và xử lý JSON (tránh lỗi vòng lặp và giữ nguyên định dạng ký tự)
    builder.Services.AddControllers(options =>
        {
            // Giữ nguyên hậu tố "Async" trong tên action để CreatedAtAction(nameof(XxxAsync)) khớp route
            options.SuppressAsyncSuffixInActionNames = false;
        })
        .AddJsonOptions(options =>
        {
            // Cho phép hiển thị các ký tự đặc biệt (tiếng Việt) mà không bị mã hóa thành \uXXXX
            options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            // Ngăn lỗi vòng lặp vô tận khi các Object tham chiếu lẫn nhau (như Category -> Product -> Category)
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        });

    // Kích hoạt FluentValidation: tự động chạy validator theo DTO khi model binding
    builder.Services.AddFluentValidationAutoValidation();
    // Quét và đăng ký toàn bộ validator trong assembly (mỗi DTO một validator)
    builder.Services.AddValidatorsFromAssembly(typeof(AutoMapperProfile).Assembly);

    // Đồng nhất định dạng lỗi validation về chung envelope ApiResponseDto (thay cho ProblemDetails mặc định)
    builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(entry => entry.Value is not null && entry.Value.Errors.Count > 0)
                .SelectMany(entry => entry.Value!.Errors.Select(error => error.ErrorMessage))
                .Where(message => !string.IsNullOrWhiteSpace(message))
                .ToList();

            var response = ApiResponseDto<object>.ErrorResult(
                "Dữ liệu không hợp lệ",
                StatusCodes.Status400BadRequest,
                errors);

            return new BadRequestObjectResult(response);
        };
    });

    // Đăng ký AutoMapper để tự động chuyển đổi dữ liệu giữa Entity và DTO
    builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

    // ĐĂNG KÝ CÁC DỊCH VỤ HỆ THỐNG (Database, JWT, IDateTime, Repositories...)
    // Hàm này nằm trong project Infrastructure
    builder.Services.AddInfrastructureServices(builder.Configuration);

    // ĐĂNG KÝ CÁC DỊCH VỤ NGHIỆP VỤ (AuthService, UserService, CategoryService...)
    // Hàm này nằm trong project Application
    builder.Services.AddApplicationServices();

    // Cấu hình CORS: Cho phép ứng dụng Frontend (React/Angular) truy cập vào API này
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend", policy =>
        {
            policy.WithOrigins("http://localhost:3000") // Địa chỉ của máy khách (Frontend)
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    });

    // Cấu hình Swagger/OpenAPI để tạo tài liệu hướng dẫn sử dụng API tự động
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new() { Title = "Management System API", Version = "v1" });

        // Cấu hình để Swagger có nút "Authorize" nhập Token JWT
        c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "Nhập token JWT của bạn theo định dạng: Bearer {token}"
        });

        c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id   = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

    // Cấu hình ProblemDetails: Xử lý lỗi tập trung và bảo mật thông tin
    builder.Services.AddProblemDetails(options =>
    {
        // CHỐT CHẶN BẢO MẬT 1: Chỉ hiện StackTrace (chi tiết lỗi) khi đang code (Dev)
        options.IncludeExceptionDetails = (ctx, ex) => builder.Environment.IsDevelopment();

        // CHỐT CHẶN BẢO MẬT 2: Map các Exception nghiệp vụ thành mã lỗi HTTP chuẩn
        options.MapToStatusCode<KeyNotFoundException>(StatusCodes.Status404NotFound);
        options.MapToStatusCode<UnauthorizedAccessException>(StatusCodes.Status401Unauthorized);
        options.MapToStatusCode<ArgumentException>(StatusCodes.Status400BadRequest);
        options.MapToStatusCode<InvalidOperationException>(StatusCodes.Status400BadRequest);

        // CHỐT CHẶN BẢO MẬT 3: Ghi đè phản hồi cho lỗi 500 (Lỗi hệ thống) ở môi trường Production
        options.OnBeforeWriteDetails = (ctx, details) =>
        {
            if (!builder.Environment.IsDevelopment() && details.Status == 500)
            {
                details.Title = "An error occurred";
                details.Detail = "Chúng tôi đã ghi nhận sự cố và đang xử lý. Vui lòng thử lại sau.";
                details.Extensions.Clear();
            }
        };
    });

    // Xây dựng ứng dụng sau khi đã đăng ký tất cả dịch vụ
    var app = builder.Build();

    // Tự động áp migration còn thiếu và seed dữ liệu nền (idempotent) khi khởi động
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync();
        await DataSeeder.SeedAsync(dbContext);
        Log.Information("Database migrated and seeded");
    }

    // ==========================================================================
    // 2. CẤU HÌNH PIPELINE (MIDDLEWARE) - Thứ tự ở đây cực kỳ quan trọng!
    // ==========================================================================

    // Kích hoạt Middleware ProblemDetails đầu tiên để bao bọc toàn bộ Pipeline
    app.UseProblemDetails();

    // Ghi log các request HTTP tự động qua Serilog
    app.UseSerilogRequestLogging();

    // Nếu là môi trường phát triển (Development) thì hiển thị giao diện hướng dẫn API
    if (app.Environment.IsDevelopment())
    {
        // Tạo file JSON mô tả API
        app.UseSwagger(options =>
        {
            options.RouteTemplate = "openapi/{documentName}.json";
        });

        // Cấu hình giao diện Scalar để xem và test API
        app.MapScalarApiReference("/docs", options =>
        {
            options.Title = "Management System API Reference";
            options.Theme = ScalarTheme.Moon;
            options.OpenApiRoutePattern = "/openapi/v1.json"; // Chỉ đường dẫn tới file JSON Swagger

            // Cấu hình xác thực trong Scalar (dùng PreferredSecuritySchemes để tránh lỗi Obsolete)
            options.Authentication = new ScalarAuthenticationOptions
            {
                PreferredSecuritySchemes = new[] { "Bearer" }
            };
        });
    }

    // Tự động chuyển hướng từ HTTP sang HTTPS để bảo mật
    app.UseHttpsRedirection();

    // Áp dụng chính sách CORS đã cấu hình ở trên
    app.UseCors("AllowFrontend");

    // Xác thực danh tính: Kiểm tra Token gửi lên có hợp lệ hay không (Ai đang gọi API?)
    app.UseAuthentication();

    // Kiểm tra quyền hạn: Người dùng này có được phép vào API này không? (Admin hay User?)
    app.UseAuthorization();

    // Khi vào trang chủ "/" sẽ tự động nhảy sang trang tài liệu API "/docs"
    app.MapGet("/", () => Results.Redirect("/docs"));

    // Ánh xạ các Controller thành các con đường dẫn (Route) API thực tế
    app.MapControllers();

    // Bắt đầu chạy ứng dụng
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}