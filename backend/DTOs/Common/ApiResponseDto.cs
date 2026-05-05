namespace ManagementSystem.DTOs.Common;

public class ApiResponseDto<T>
{
    public int StatusCode { get; set; }
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;
    public string? Error { get; set; }

    public T? Data { get; set; }

    public List<string>? Errors { get; set; }

    public static ApiResponseDto<T> SuccessResult(
        T data,
        string message = "Success",
        int statusCode = 200)
    {
        return new ApiResponseDto<T>
        {
            StatusCode = statusCode,
            Success = true,
            Message = message,
            Data = data
        };
    }

    public static ApiResponseDto<T> ErrorResult(
        string message,
        int statusCode = 400,
        List<string>? errors = null)
    {
        return new ApiResponseDto<T>
        {
            StatusCode = statusCode,
            Success = false,
            Message = message,
            Error = message,
            Errors = errors
        };
    }
}

public class ApiResponseDto
{
    public int StatusCode { get; set; }
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;
    public string? Error { get; set; }

    public List<string>? Errors { get; set; }

    public static ApiResponseDto SuccessResult(
        string message = "Success",
        int statusCode = 200)
    {
        return new ApiResponseDto
        {
            StatusCode = statusCode,
            Success = true,
            Message = message
        };
    }

    public static ApiResponseDto ErrorResult(
        string message,
        int statusCode = 400,
        List<string>? errors = null)
    {
        return new ApiResponseDto
        {
            StatusCode = statusCode,
            Success = false,
            Message = message,
            Error = message,
            Errors = errors
        };
    }
}