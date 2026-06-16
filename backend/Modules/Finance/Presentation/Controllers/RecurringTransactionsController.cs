using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Finance.Application.DTOs;

namespace ManagementSystem.Modules.Finance.Presentation.Controllers;

[ApiController]
[Route("api/recurring-transactions")]
[Authorize]
public class RecurringTransactionsController : ControllerBase
{
    private readonly IRecurringTransactionService _service;
    private readonly ILogger<RecurringTransactionsController> _logger;

    public RecurringTransactionsController(IRecurringTransactionService service, ILogger<RecurringTransactionsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponseDto<PaginatedResultDto<RecurringTransactionDto>>>> GetAllAsync([FromQuery] RecurringTransactionQueryParams query)
    {
        try
        {
            var result = await _service.GetPagedAsync(query);
            return Ok(ApiResponseDto<PaginatedResultDto<RecurringTransactionDto>>.SuccessResult(result, "Lấy danh sách giao dịch định kỳ thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting recurring transactions");
            return StatusCode(500, ApiResponseDto<PaginatedResultDto<RecurringTransactionDto>>.ErrorResult("Có lỗi xảy ra khi lấy danh sách giao dịch định kỳ", 500));
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<RecurringTransactionDto>>> GetByIdAsync(Guid id)
    {
        try
        {
            var item = await _service.GetByIdAsync(id);
            if (item is null)
                return NotFound(ApiResponseDto<RecurringTransactionDto>.ErrorResult("Không tìm thấy giao dịch định kỳ", 404));
            return Ok(ApiResponseDto<RecurringTransactionDto>.SuccessResult(item, "Lấy thông tin giao dịch định kỳ thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting recurring transaction {Id}", id);
            return StatusCode(500, ApiResponseDto<RecurringTransactionDto>.ErrorResult("Có lỗi xảy ra khi lấy thông tin giao dịch định kỳ", 500));
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<RecurringTransactionDto>>> CreateAsync([FromBody] CreateRecurringTransactionDto dto)
    {
        try
        {
            var item = await _service.CreateAsync(dto);
            if (item is null)
                return NotFound(ApiResponseDto<RecurringTransactionDto>.ErrorResult("Không tìm thấy tài khoản", 404));
            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id },
                ApiResponseDto<RecurringTransactionDto>.SuccessResult(item, "Tạo giao dịch định kỳ thành công", 201));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating recurring transaction");
            return StatusCode(500, ApiResponseDto<RecurringTransactionDto>.ErrorResult("Có lỗi xảy ra khi tạo giao dịch định kỳ", 500));
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<RecurringTransactionDto>>> UpdateAsync(Guid id, [FromBody] UpdateRecurringTransactionDto dto)
    {
        try
        {
            var item = await _service.UpdateAsync(id, dto);
            if (item is null)
                return NotFound(ApiResponseDto<RecurringTransactionDto>.ErrorResult("Không tìm thấy giao dịch định kỳ hoặc tài khoản", 404));
            return Ok(ApiResponseDto<RecurringTransactionDto>.SuccessResult(item, "Cập nhật giao dịch định kỳ thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating recurring transaction {Id}", id);
            return StatusCode(500, ApiResponseDto<RecurringTransactionDto>.ErrorResult("Có lỗi xảy ra khi cập nhật giao dịch định kỳ", 500));
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
                return NotFound(ApiResponseDto<bool>.ErrorResult("Không tìm thấy giao dịch định kỳ", 404));
            return Ok(ApiResponseDto<bool>.SuccessResult(result, "Xóa giao dịch định kỳ thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting recurring transaction {Id}", id);
            return StatusCode(500, ApiResponseDto<bool>.ErrorResult("Có lỗi xảy ra khi xóa giao dịch định kỳ", 500));
        }
    }
}
