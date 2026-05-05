using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ManagementSystem.DTOs;
using ManagementSystem.DTOs.Auth;
using ManagementSystem.DTOs.Common;
using ManagementSystem.DTOs.User;
using ManagementSystem.Interfaces;

namespace ManagementSystem.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IAuthService authService,
            IUserService userService,
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// User login
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponseDto<AuthResponse>>> Login([FromBody] LoginDto request)
        {
            Console.WriteLine(BCrypt.Net.BCrypt.HashPassword("123456"));

            try
            {
                var result = await _authService.LoginAsync(request);
                return Ok(ApiResponseDto<AuthResponse>.SuccessResult(result, "Đăng nhập thành công"));
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Login failed for email: {Email}", request.Email);
                return Unauthorized(ApiResponseDto<AuthResponse>.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LOGIN ERROR] {ex.GetType().Name}: {ex.Message}");
                Console.WriteLine($"[STACK] {ex.StackTrace}");
                if (ex.InnerException != null)
                    Console.WriteLine($"[INNER] {ex.InnerException.Message}");

                _logger.LogError(ex, "Error during login");
                return StatusCode(500, ApiResponseDto<AuthResponse>.ErrorResult("Có lỗi xảy ra khi đăng nhập"));
            }
        }

        /// <summary>
        /// Refresh access token
        /// </summary>
        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponseDto<AuthResponse>>> Refresh([FromBody] string refreshToken)
        {
            try
            {
                var result = await _authService.RefreshTokenAsync(refreshToken);
                return Ok(ApiResponseDto<AuthResponse>.SuccessResult(result, "Làm mới token thành công"));
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Token refresh failed");
                return Unauthorized(ApiResponseDto<AuthResponse>.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token refresh");
                return StatusCode(500, ApiResponseDto<AuthResponse>.ErrorResult("Có lỗi xảy ra khi làm mới token"));
            }
        }

        /// <summary>
        /// User logout
        /// </summary>
        [HttpPost("logout")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponseDto>> Logout([FromBody] string? refreshToken = null)
        {
            try
            {
                await _authService.LogoutAsync(refreshToken);
                return Ok(ApiResponseDto.SuccessResult("Đăng xuất thành công"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                return StatusCode(500, ApiResponseDto.ErrorResult("Có lỗi xảy ra khi đăng xuất"));
            }
        }

        /// <summary>
        /// Register new user
        /// </summary>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponseDto<UserDto>>> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var result = await _userService.RegisterAsync(request);
                return CreatedAtAction(nameof(Register), new { id = result.Id },
                    ApiResponseDto<UserDto>.SuccessResult(result, "Đăng ký thành công"));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Registration failed: {Message}", ex.Message);
                return BadRequest(ApiResponseDto<UserDto>.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration");
                return StatusCode(500, ApiResponseDto<UserDto>.ErrorResult("Có lỗi xảy ra khi đăng ký"));
            }
        }
    }
}