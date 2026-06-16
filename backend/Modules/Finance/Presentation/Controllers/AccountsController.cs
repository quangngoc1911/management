using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Finance.Application.DTOs;

namespace ManagementSystem.Modules.Finance.Presentation.Controllers;

[ApiController]
[Route("api/accounts")]
[Authorize]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly ILogger<AccountsController> _logger;

    public AccountsController(IAccountService accountService, ILogger<AccountsController> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponseDto<PaginatedResultDto<AccountDto>>>> GetAllAsync([FromQuery] AccountQueryParams query)
    {
        try
        {
            var result = await _accountService.GetPagedAsync(query);
            return Ok(ApiResponseDto<PaginatedResultDto<AccountDto>>.SuccessResult(result, "Lấy danh sách tài khoản thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting accounts");
            return StatusCode(500, ApiResponseDto<PaginatedResultDto<AccountDto>>.ErrorResult("Có lỗi xảy ra khi lấy danh sách tài khoản", 500));
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<AccountDto>>> GetByIdAsync(Guid id)
    {
        try
        {
            var account = await _accountService.GetByIdAsync(id);
            if (account is null)
            {
                return NotFound(ApiResponseDto<AccountDto>.ErrorResult("Không tìm thấy tài khoản", 404));
            }
            return Ok(ApiResponseDto<AccountDto>.SuccessResult(account, "Lấy thông tin tài khoản thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting account {AccountId}", id);
            return StatusCode(500, ApiResponseDto<AccountDto>.ErrorResult("Có lỗi xảy ra khi lấy thông tin tài khoản", 500));
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<AccountDto>>> CreateAsync([FromBody] CreateAccountDto dto)
    {
        try
        {
            var account = await _accountService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = account.Id },
                ApiResponseDto<AccountDto>.SuccessResult(account, "Tạo tài khoản thành công", 201));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating account");
            return StatusCode(500, ApiResponseDto<AccountDto>.ErrorResult("Có lỗi xảy ra khi tạo tài khoản", 500));
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<AccountDto>>> UpdateAsync(Guid id, [FromBody] UpdateAccountDto dto)
    {
        try
        {
            var account = await _accountService.UpdateAsync(id, dto);
            if (account is null)
            {
                return NotFound(ApiResponseDto<AccountDto>.ErrorResult("Không tìm thấy tài khoản", 404));
            }
            return Ok(ApiResponseDto<AccountDto>.SuccessResult(account, "Cập nhật tài khoản thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating account {AccountId}", id);
            return StatusCode(500, ApiResponseDto<AccountDto>.ErrorResult("Có lỗi xảy ra khi cập nhật tài khoản", 500));
        }
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponseDto<bool>>> DeleteAsync(Guid id)
    {
        try
        {
            var result = await _accountService.DeleteAsync(id);
            if (!result)
            {
                return NotFound(ApiResponseDto<bool>.ErrorResult("Không tìm thấy tài khoản", 404));
            }
            return Ok(ApiResponseDto<bool>.SuccessResult(result, "Xóa tài khoản thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting account {AccountId}", id);
            return StatusCode(500, ApiResponseDto<bool>.ErrorResult("Có lỗi xảy ra khi xóa tài khoản", 500));
        }
    }
}
