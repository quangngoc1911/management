using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Finance.Application.DTOs;

namespace ManagementSystem.Modules.Finance.Presentation.Controllers;

[ApiController]
[Route("api/investments")]
[Authorize]
public class InvestmentsController : ControllerBase
{
    private readonly IInvestmentService _investmentService;
    private readonly ILogger<InvestmentsController> _logger;

    public InvestmentsController(IInvestmentService investmentService, ILogger<InvestmentsController> logger)
    {
        _investmentService = investmentService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponseDto<PaginatedResultDto<InvestmentDto>>>> GetAllAsync([FromQuery] InvestmentQueryParams query)
    {
        try
        {
            var result = await _investmentService.GetPagedAsync(query);
            return Ok(ApiResponseDto<PaginatedResultDto<InvestmentDto>>.SuccessResult(result, "Lấy danh sách đầu tư thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting investments");
            return StatusCode(500, ApiResponseDto<PaginatedResultDto<InvestmentDto>>.ErrorResult("Có lỗi xảy ra khi lấy danh sách đầu tư", 500));
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<InvestmentDto>>> GetByIdAsync(Guid id)
    {
        try
        {
            var investment = await _investmentService.GetByIdAsync(id);
            if (investment is null)
                return NotFound(ApiResponseDto<InvestmentDto>.ErrorResult("Không tìm thấy khoản đầu tư", 404));
            return Ok(ApiResponseDto<InvestmentDto>.SuccessResult(investment, "Lấy thông tin đầu tư thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting investment {InvestmentId}", id);
            return StatusCode(500, ApiResponseDto<InvestmentDto>.ErrorResult("Có lỗi xảy ra khi lấy thông tin đầu tư", 500));
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<InvestmentDto>>> CreateAsync([FromBody] CreateInvestmentDto dto)
    {
        try
        {
            var investment = await _investmentService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = investment.Id },
                ApiResponseDto<InvestmentDto>.SuccessResult(investment, "Tạo khoản đầu tư thành công", 201));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating investment");
            return StatusCode(500, ApiResponseDto<InvestmentDto>.ErrorResult("Có lỗi xảy ra khi tạo khoản đầu tư", 500));
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<InvestmentDto>>> UpdateAsync(Guid id, [FromBody] UpdateInvestmentDto dto)
    {
        try
        {
            var investment = await _investmentService.UpdateAsync(id, dto);
            if (investment is null)
                return NotFound(ApiResponseDto<InvestmentDto>.ErrorResult("Không tìm thấy khoản đầu tư", 404));
            return Ok(ApiResponseDto<InvestmentDto>.SuccessResult(investment, "Cập nhật khoản đầu tư thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating investment {InvestmentId}", id);
            return StatusCode(500, ApiResponseDto<InvestmentDto>.ErrorResult("Có lỗi xảy ra khi cập nhật khoản đầu tư", 500));
        }
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponseDto<bool>>> DeleteAsync(Guid id)
    {
        try
        {
            var result = await _investmentService.DeleteAsync(id);
            if (!result)
                return NotFound(ApiResponseDto<bool>.ErrorResult("Không tìm thấy khoản đầu tư", 404));
            return Ok(ApiResponseDto<bool>.SuccessResult(result, "Xóa khoản đầu tư thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting investment {InvestmentId}", id);
            return StatusCode(500, ApiResponseDto<bool>.ErrorResult("Có lỗi xảy ra khi xóa khoản đầu tư", 500));
        }
    }
}
