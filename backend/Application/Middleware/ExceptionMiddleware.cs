using System.Net;
using System.Text.Json;

namespace ManagementSystem.Application.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message) = ex switch
        {
            KeyNotFoundException => (404, ex.Message),
            UnauthorizedAccessException => (401, ex.Message),
            ArgumentException => (400, ex.Message),
            InvalidOperationException => (400, ex.Message),
            _ => (500, "Lỗi hệ thống, vui lòng thử lại sau")
        };

        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsJsonAsync(new
        {
            statusCode,
            message,
            detail = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
                ? ex.InnerException?.Message ?? ex.StackTrace
                : null
        });
    }

}
