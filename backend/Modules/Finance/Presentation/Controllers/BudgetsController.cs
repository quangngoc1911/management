using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Finance.Application.DTOs;

namespace ManagementSystem.Modules.Finance.Presentation.Controllers;

[ApiController]
[Route("api/budgets")]
[Authorize]
public class BudgetsController : ControllerBase
{
    private readonly IBudgetService _budgetService;
    private readonly ILogger<BudgetsController> _logger;

    public BudgetsController(IBudgetService budgetService, ILogger<BudgetsController> logger)
    {
        _budgetService = budgetService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponseDto<PaginatedResultDto<BudgetDto>>>> GetAllAsync([FromQuery] BudgetQueryParams query)
    {
        try
        {
            var result = await _budgetService.GetPagedAsync(query);
            return Ok(ApiResponseDto<PaginatedResultDto<BudgetDto>>.SuccessResult(result, "Lấy danh sách ngân sách thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting budgets");
            return StatusCode(500, ApiResponseDto<PaginatedResultDto<BudgetDto>>.ErrorResult("Có lỗi xảy ra khi lấy danh sách ngân sách", 500));
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<BudgetDto>>> GetByIdAsync(Guid id)
    {
        try
        {
            var budget = await _budgetService.GetByIdAsync(id);
            if (budget is null)
                return NotFound(ApiResponseDto<BudgetDto>.ErrorResult("Không tìm thấy ngân sách", 404));
            return Ok(ApiResponseDto<BudgetDto>.SuccessResult(budget, "Lấy thông tin ngân sách thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting budget {BudgetId}", id);
            return StatusCode(500, ApiResponseDto<BudgetDto>.ErrorResult("Có lỗi xảy ra khi lấy thông tin ngân sách", 500));
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<BudgetDto>>> CreateAsync([FromBody] CreateBudgetDto dto)
    {
        try
        {
            var budget = await _budgetService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = budget.Id },
                ApiResponseDto<BudgetDto>.SuccessResult(budget, "Tạo ngân sách thành công", 201));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating budget");
            return StatusCode(500, ApiResponseDto<BudgetDto>.ErrorResult("Có lỗi xảy ra khi tạo ngân sách", 500));
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<BudgetDto>>> UpdateAsync(Guid id, [FromBody] UpdateBudgetDto dto)
    {
        try
        {
            var budget = await _budgetService.UpdateAsync(id, dto);
            if (budget is null)
                return NotFound(ApiResponseDto<BudgetDto>.ErrorResult("Không tìm thấy ngân sách", 404));
            return Ok(ApiResponseDto<BudgetDto>.SuccessResult(budget, "Cập nhật ngân sách thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating budget {BudgetId}", id);
            return StatusCode(500, ApiResponseDto<BudgetDto>.ErrorResult("Có lỗi xảy ra khi cập nhật ngân sách", 500));
        }
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponseDto<bool>>> DeleteAsync(Guid id)
    {
        try
        {
            var result = await _budgetService.DeleteAsync(id);
            if (!result)
                return NotFound(ApiResponseDto<bool>.ErrorResult("Không tìm thấy ngân sách", 404));
            return Ok(ApiResponseDto<bool>.SuccessResult(result, "Xóa ngân sách thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting budget {BudgetId}", id);
            return StatusCode(500, ApiResponseDto<bool>.ErrorResult("Có lỗi xảy ra khi xóa ngân sách", 500));
        }
    }
}
