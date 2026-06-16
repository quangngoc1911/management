using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Family.Application.DTOs;

namespace ManagementSystem.Modules.Family.Presentation.Controllers;

[ApiController]
[Route("api/member-relationships")]
[Authorize]
public class MemberRelationshipsController : ControllerBase
{
    private readonly IMemberRelationshipService _service;
    private readonly ILogger<MemberRelationshipsController> _logger;

    public MemberRelationshipsController(IMemberRelationshipService service, ILogger<MemberRelationshipsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponseDto<List<MemberRelationshipDto>>>> GetByMemberAsync([FromQuery] Guid memberId)
    {
        try
        {
            var result = await _service.GetByMemberAsync(memberId);
            if (result is null)
            {
                return NotFound(ApiResponseDto<List<MemberRelationshipDto>>.ErrorResult("Không tìm thấy thành viên gia đình", 404));
            }
            return Ok(ApiResponseDto<List<MemberRelationshipDto>>.SuccessResult(result, "Lấy danh sách quan hệ thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting relationships for member {MemberId}", memberId);
            return StatusCode(500, ApiResponseDto<List<MemberRelationshipDto>>.ErrorResult("Có lỗi xảy ra khi lấy danh sách quan hệ", 500));
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<MemberRelationshipDto>>> CreateAsync([FromBody] CreateMemberRelationshipDto dto)
    {
        try
        {
            var result = await _service.CreateAsync(dto);
            if (result is null)
            {
                return NotFound(ApiResponseDto<MemberRelationshipDto>.ErrorResult("Không tìm thấy thành viên gia đình", 404));
            }
            return Ok(ApiResponseDto<MemberRelationshipDto>.SuccessResult(result, "Tạo quan hệ thành công", 201));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponseDto<MemberRelationshipDto>.ErrorResult(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating member relationship");
            return StatusCode(500, ApiResponseDto<MemberRelationshipDto>.ErrorResult("Có lỗi xảy ra khi tạo quan hệ", 500));
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<MemberRelationshipDto>>> UpdateAsync(Guid id, [FromBody] UpdateMemberRelationshipDto dto)
    {
        try
        {
            var result = await _service.UpdateAsync(id, dto);
            if (result is null)
            {
                return NotFound(ApiResponseDto<MemberRelationshipDto>.ErrorResult("Không tìm thấy quan hệ", 404));
            }
            return Ok(ApiResponseDto<MemberRelationshipDto>.SuccessResult(result, "Cập nhật quan hệ thành công"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponseDto<MemberRelationshipDto>.ErrorResult(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating member relationship {Id}", id);
            return StatusCode(500, ApiResponseDto<MemberRelationshipDto>.ErrorResult("Có lỗi xảy ra khi cập nhật quan hệ", 500));
        }
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponseDto<bool>>> DeleteAsync(Guid id)
    {
        try
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
            {
                return NotFound(ApiResponseDto<bool>.ErrorResult("Không tìm thấy quan hệ", 404));
            }
            return Ok(ApiResponseDto<bool>.SuccessResult(result, "Xóa quan hệ thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting member relationship {Id}", id);
            return StatusCode(500, ApiResponseDto<bool>.ErrorResult("Có lỗi xảy ra khi xóa quan hệ", 500));
        }
    }
}
