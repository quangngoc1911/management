using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ManagementSystem.DTOs.User;
using ManagementSystem.Interfaces;
using ManagementSystem.DTOs.Common;

namespace ManagementSystem.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService service, ILogger<UserController> logger)
    {
        _service = service;
        _logger = logger;
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("User ID not found in token");
        }
        return userId;
    }

    /// <summary>
    /// Get all users with pagination
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponseDto<PaginatedResultDto<UserDto>>>> GetAllUsersAsync([FromQuery] PageRequest request)
    {
        try
        {
            var users = await _service.GetAllUsersAsync(request);
            return Ok(ApiResponseDto<PaginatedResultDto<UserDto>>.SuccessResult(users, "Lấy danh sách người dùng thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all users");
            return StatusCode(500, ApiResponseDto<PaginatedResultDto<UserDto>>.ErrorResult("Có lỗi xảy ra khi lấy danh sách người dùng"));
        }
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Admin,Editor,Viewer")] // Adjust roles as needed
    public async Task<ActionResult<ApiResponseDto<UserDto>>> GetUserByIdAsync(Guid id)
    {
        try
        {
            var user = await _service.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(ApiResponseDto<UserDto>.ErrorResult("Không tìm thấy người dùng"));
            }
            // Ensure non-admin users can only view their own profile
            if (!User.IsInRole("Admin") && user.Id != GetCurrentUserId())
            {
                return Forbid();
            }
            return Ok(ApiResponseDto<UserDto>.SuccessResult(user, "Lấy thông tin người dùng thành công"));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ApiResponseDto<UserDto>.ErrorResult(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user {UserId}", id);
            return StatusCode(500, ApiResponseDto<UserDto>.ErrorResult("Có lỗi xảy ra khi lấy thông tin người dùng"));
        }
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponseDto<UserDto>>> CreateUserAsync([FromBody] CreateUserDto dto)
    {
        try
        {
            var createdBy = GetCurrentUserId();
            var user = await _service.CreateUserAsync(dto, createdBy);
            return CreatedAtAction(
                nameof(GetUserByIdAsync),
                new { id = user.Id },
                ApiResponseDto<UserDto>.SuccessResult(user, "Tạo người dùng thành công")
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponseDto<UserDto>.ErrorResult(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ApiResponseDto<UserDto>.ErrorResult(ex.Message));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ApiResponseDto<UserDto>.ErrorResult(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user");
            return StatusCode(500, ApiResponseDto<UserDto>.ErrorResult("Có lỗi xảy ra khi tạo người dùng"));
        }
    }

    /// <summary>
    /// Update an existing user
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Editor")] // Allow editors to update their own profile
    public async Task<ActionResult<ApiResponseDto<UserDto>>> UpdateUserAsync(Guid id, [FromBody] UpdateUserDto dto)
    {
        try
        {
            // Non-admin users can only update their own profile
            if (!User.IsInRole("Admin") && id != GetCurrentUserId())
            {
                return Forbid();
            }

            var updatedBy = GetCurrentUserId();
            var user = await _service.UpdateUserAsync(id, dto, updatedBy);
            if (user == null)
            {
                return NotFound(ApiResponseDto<UserDto>.ErrorResult("Không tìm thấy người dùng"));
            }
            return Ok(ApiResponseDto<UserDto>.SuccessResult(user, "Cập nhật người dùng thành công"));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponseDto<UserDto>.ErrorResult(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ApiResponseDto<UserDto>.ErrorResult(ex.Message));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ApiResponseDto<UserDto>.ErrorResult(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user {UserId}", id);
            return StatusCode(500, ApiResponseDto<UserDto>.ErrorResult("Có lỗi xảy ra khi cập nhật người dùng"));
        }
    }

    /// <summary>
    /// Delete a user
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponseDto<bool>>> DeleteUserAsync(Guid id)
    {
        try
        {
            var result = await _service.DeleteUserAsync(id);
            if (!result)
            {
                return NotFound(ApiResponseDto<bool>.ErrorResult("Không tìm thấy người dùng"));
            }
            return Ok(ApiResponseDto<bool>.SuccessResult(result, "Xóa người dùng thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user {UserId}", id);
            return StatusCode(500, ApiResponseDto<bool>.ErrorResult("Có lỗi xảy ra khi xóa người dùng"));
        }
    }
}
