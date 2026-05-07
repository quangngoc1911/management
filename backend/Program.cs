using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using ManagementSystem.Application;
using ManagementSystem.Infrastructure;
using ManagementSystem.Application.Mappings;
using ManagementSystem.Application.Middleware;

using Scalar.AspNetCore;

// Đảm bảo Console hiển thị được tiếng Việt có dấu khi Log thông tin
Console.OutputEncoding = Encoding.UTF8;

var builder = WebApplication.CreateBuilder(args);

// Hiển thị môi trường đang chạy (Development, Staging, hoặc Production)
Console.WriteLine($"Môi trường hiện tại: {builder.Environment.EnvironmentName}");

// ==========================================================================
// 1. CẤU HÌNH DỊCH VỤ (DEPENDENCY INJECTION - DI)
// ==========================================================================

// Cấu hình Controllers và xử lý JSON (tránh lỗi vòng lặp và giữ nguyên định dạng ký tự)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Cho phép hiển thị các ký tự đặc biệt (tiếng Việt) mà không bị mã hóa thành \uXXXX
        options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        // Ngăn lỗi vòng lặp vô tận khi các Object tham chiếu lẫn nhau (như Category -> Product -> Category)
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
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

// Xây dựng ứng dụng sau khi đã đăng ký tất cả dịch vụ
var app = builder.Build();

// ==========================================================================
// 2. CẤU HÌNH PIPELINE (MIDDLEWARE) - Thứ tự ở đây cực kỳ quan trọng!
// ==========================================================================

// Nếu là môi trường phát triển (Development) thì hiển thị giao diện hướng dẫn API
if (app.Environment.IsDevelopment())
{
    // Tạo file JSON mô tả API
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "openapi/{documentName}.json";
    });

    // Cấu hình giao diện Scalar (đẹp hơn Swagger UI) để xem và test API
    app.MapScalarApiReference("/docs", options =>
    {
        options.Title = "Management System API Reference";
        options.Theme = ScalarTheme.Moon; // Giao diện tối (Dark Mode)
        options.OpenApiRoutePattern = "/openapi/v1.json"; // Chỉ đường dẫn tới file JSON Swagger

        // Cấu hình xác thực trong Scalar (dùng PreferredSecuritySchemes để tránh lỗi Obsolete)
        options.Authentication = new ScalarAuthenticationOptions
        {
            PreferredSecuritySchemes = new[] { "Bearer" }
        };
    });
}

// Xử lý lỗi tập trung: Mọi lỗi xảy ra trong code sẽ được bắt tại đây và trả về JSON chuẩn
app.UseMiddleware<ExceptionMiddleware>();

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