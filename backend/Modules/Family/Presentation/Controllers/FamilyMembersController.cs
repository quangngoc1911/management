using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Family.Application.DTOs;

namespace ManagementSystem.Modules.Family.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FamilyMembersController : ControllerBase
{
    private readonly IFamilyMemberService _familyMemberService;
    private readonly ILogger<FamilyMembersController> _logger;

    public FamilyMembersController(
        IFamilyMemberService familyMemberService,
        ILogger<FamilyMembersController> logger)
    {
        _familyMemberService = familyMemberService;
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

    [HttpGet]
    public async Task<ActionResult<ApiResponseDto<PaginatedResultDto<FamilyMemberDto>>>> GetAllAsync(
        [FromQuery] FamilyMemberQueryParams query)
    {
        try
        {
            var result = await _familyMemberService.GetPagedAsync(query);
            return Ok(ApiResponseDto<PaginatedResultDto<FamilyMemberDto>>.SuccessResult(
                result, "Lấy danh sách thành viên gia đình thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting family members");
            return StatusCode(500, ApiResponseDto<PaginatedResultDto<FamilyMemberDto>>.ErrorResult(
                "Có lỗi xảy ra khi lấy danh sách thành viên gia đình", 500));
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<FamilyMemberDto>>> GetByIdAsync(Guid id)
    {
        try
        {
            var member = await _familyMemberService.GetByIdAsync(id);
            if (member is null)
            {
                return NotFound(ApiResponseDto<FamilyMemberDto>.ErrorResult("Không tìm thấy thành viên gia đình", 404));
            }
            return Ok(ApiResponseDto<FamilyMemberDto>.SuccessResult(member, "Lấy thông tin thành viên gia đình thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting family member {FamilyMemberId}", id);
            return StatusCode(500, ApiResponseDto<FamilyMemberDto>.ErrorResult(
                "Có lỗi xảy ra khi lấy thông tin thành viên gia đình", 500));
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<FamilyMemberDto>>> CreateAsync(
        [FromBody] CreateFamilyMemberDto dto)
    {
        try
        {
            dto.CreatedBy = GetCurrentUserId();
            var member = await _familyMemberService.CreateAsync(dto);
            return CreatedAtAction(
                nameof(GetByIdAsync),
                new { id = member.Id },
                ApiResponseDto<FamilyMemberDto>.SuccessResult(member, "Tạo thành viên gia đình thành công", 201));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating family member");
            return StatusCode(500, ApiResponseDto<FamilyMemberDto>.ErrorResult(
                "Có lỗi xảy ra khi tạo thành viên gia đình", 500));
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<FamilyMemberDto>>> UpdateAsync(
        Guid id,
        [FromBody] UpdateFamilyMemberDto dto)
    {
        try
        {
            dto.UpdatedBy = GetCurrentUserId();
            var member = await _familyMemberService.UpdateAsync(id, dto);
            if (member is null)
            {
                return NotFound(ApiResponseDto<FamilyMemberDto>.ErrorResult("Không tìm thấy thành viên gia đình", 404));
            }
            return Ok(ApiResponseDto<FamilyMemberDto>.SuccessResult(member, "Cập nhật thành viên gia đình thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating family member {FamilyMemberId}", id);
            return StatusCode(500, ApiResponseDto<FamilyMemberDto>.ErrorResult(
                "Có lỗi xảy ra khi cập nhật thành viên gia đình", 500));
        }
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponseDto<bool>>> DeleteAsync(Guid id)
    {
        try
        {
            var result = await _familyMemberService.DeleteAsync(id);
            if (!result)
            {
                return NotFound(ApiResponseDto<bool>.ErrorResult("Không tìm thấy thành viên gia đình", 404));
            }
            return Ok(ApiResponseDto<bool>.SuccessResult(result, "Xóa thành viên gia đình thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting family member {FamilyMemberId}", id);
            return StatusCode(500, ApiResponseDto<bool>.ErrorResult(
                "Có lỗi xảy ra khi xóa thành viên gia đình", 500));
        }
    }
}
