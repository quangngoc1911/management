using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ManagementSystem.Application.Options;
using ManagementSystem.Infrastructure.Persistence;
using ManagementSystem.Infrastructure.Interceptors;
using ManagementSystem.Application.Contracts;
using ManagementSystem.Infrastructure.Services; // 1. Thêm namespace chứa Interceptor

namespace ManagementSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // 2. Đăng ký Interceptor vào DI
        services.AddScoped<UpdateAuditableEntitiesInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            // 3. Lấy Interceptor từ ServiceProvider
            var auditableInterceptor = sp.GetRequiredService<UpdateAuditableEntitiesInterceptor>();

            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                   .AddInterceptors(auditableInterceptor); // 4. Đăng ký Interceptor vào DbContext
        });

        // --- Giữ nguyên phần cấu hình JWT phía dưới ---
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        var jwt = configuration.GetSection("JwtSettings").Get<JwtSettings>()!;
        var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
            ?? jwt.SecretKey
            ?? throw new InvalidOperationException("JWT SecretKey chưa được cấu hình");

        var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER")
            ?? configuration["JwtSettings:Issuer"]!;

        var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
            ?? configuration["JwtSettings:Audience"]!;

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsJsonAsync(new
                        {
                            message = "Chưa đăng nhập hoặc token hết hạn"
                        });
                    },
                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsJsonAsync(new
                        {
                            message = "Không có quyền truy cập"
                        });
                    }
                };
            });

        return services;
    }
}