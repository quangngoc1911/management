using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Family.Application.DTOs;

namespace ManagementSystem.Modules.Family.Presentation.Controllers;

[ApiController]
[Route("api/family-members/{memberId:guid}/profile")]
[Authorize]
public class MemberProfilesController : ControllerBase
{
    private readonly IMemberProfileService _memberProfileService;
    private readonly ILogger<MemberProfilesController> _logger;

    public MemberProfilesController(
        IMemberProfileService memberProfileService,
        ILogger<MemberProfilesController> logger)
    {
        _memberProfileService = memberProfileService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponseDto<MemberProfileDto>>> GetAsync(Guid memberId)
    {
        try
        {
            var profile = await _memberProfileService.GetByMemberIdAsync(memberId);
            if (profile is null)
            {
                return NotFound(ApiResponseDto<MemberProfileDto>.ErrorResult("Chưa có hồ sơ chi tiết cho thành viên này", 404));
            }
            return Ok(ApiResponseDto<MemberProfileDto>.SuccessResult(profile, "Lấy hồ sơ chi tiết thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting profile for member {MemberId}", memberId);
            return StatusCode(500, ApiResponseDto<MemberProfileDto>.ErrorResult("Có lỗi xảy ra khi lấy hồ sơ chi tiết", 500));
        }
    }

    [HttpPut]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<MemberProfileDto>>> UpsertAsync(
        Guid memberId,
        [FromBody] UpsertMemberProfileDto dto)
    {
        try
        {
            var profile = await _memberProfileService.UpsertAsync(memberId, dto);
            if (profile is null)
            {
                return NotFound(ApiResponseDto<MemberProfileDto>.ErrorResult("Không tìm thấy thành viên gia đình", 404));
            }
            return Ok(ApiResponseDto<MemberProfileDto>.SuccessResult(profile, "Lưu hồ sơ chi tiết thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error upserting profile for member {MemberId}", memberId);
            return StatusCode(500, ApiResponseDto<MemberProfileDto>.ErrorResult("Có lỗi xảy ra khi lưu hồ sơ chi tiết", 500));
        }
    }
}
