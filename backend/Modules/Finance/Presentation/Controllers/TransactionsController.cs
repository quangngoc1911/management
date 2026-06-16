using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Finance.Application.DTOs;

namespace ManagementSystem.Modules.Finance.Presentation.Controllers;

[ApiController]
[Route("api/transactions")]
[Authorize]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly ILogger<TransactionsController> _logger;

    public TransactionsController(ITransactionService transactionService, ILogger<TransactionsController> logger)
    {
        _transactionService = transactionService;
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
    public async Task<ActionResult<ApiResponseDto<PaginatedResultDto<TransactionDto>>>> GetAllAsync([FromQuery] TransactionQueryParams query)
    {
        try
        {
            var result = await _transactionService.GetPagedAsync(query);
            return Ok(ApiResponseDto<PaginatedResultDto<TransactionDto>>.SuccessResult(result, "Lấy danh sách giao dịch thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting transactions");
            return StatusCode(500, ApiResponseDto<PaginatedResultDto<TransactionDto>>.ErrorResult("Có lỗi xảy ra khi lấy danh sách giao dịch", 500));
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponseDto<TransactionDto>>> GetByIdAsync(Guid id)
    {
        try
        {
            var transaction = await _transactionService.GetByIdAsync(id);
            if (transaction is null)
            {
                return NotFound(ApiResponseDto<TransactionDto>.ErrorResult("Không tìm thấy giao dịch", 404));
            }
            return Ok(ApiResponseDto<TransactionDto>.SuccessResult(transaction, "Lấy thông tin giao dịch thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting transaction {TransactionId}", id);
            return StatusCode(500, ApiResponseDto<TransactionDto>.ErrorResult("Có lỗi xảy ra khi lấy thông tin giao dịch", 500));
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<TransactionDto>>> CreateAsync([FromBody] CreateTransactionDto dto)
    {
        try
        {
            dto.CreatedBy = GetCurrentUserId();
            var transaction = await _transactionService.CreateAsync(dto);
            if (transaction is null)
            {
                return NotFound(ApiResponseDto<TransactionDto>.ErrorResult("Không tìm thấy tài khoản", 404));
            }
            return CreatedAtAction(nameof(GetByIdAsync), new { id = transaction.Id },
                ApiResponseDto<TransactionDto>.SuccessResult(transaction, "Tạo giao dịch thành công", 201));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating transaction");
            return StatusCode(500, ApiResponseDto<TransactionDto>.ErrorResult("Có lỗi xảy ra khi tạo giao dịch", 500));
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<ApiResponseDto<TransactionDto>>> UpdateAsync(Guid id, [FromBody] UpdateTransactionDto dto)
    {
        try
        {
            var transaction = await _transactionService.UpdateAsync(id, dto);
            if (transaction is null)
            {
                return NotFound(ApiResponseDto<TransactionDto>.ErrorResult("Không tìm thấy giao dịch hoặc tài khoản", 404));
            }
            return Ok(ApiResponseDto<TransactionDto>.SuccessResult(transaction, "Cập nhật giao dịch thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating transaction {TransactionId}", id);
            return StatusCode(500, ApiResponseDto<TransactionDto>.ErrorResult("Có lỗi xảy ra khi cập nhật giao dịch", 500));
        }
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponseDto<bool>>> DeleteAsync(Guid id)
    {
        try
        {
            var result = await _transactionService.DeleteAsync(id);
            if (!result)
            {
                return NotFound(ApiResponseDto<bool>.ErrorResult("Không tìm thấy giao dịch", 404));
            }
            return Ok(ApiResponseDto<bool>.SuccessResult(result, "Xóa giao dịch thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting transaction {TransactionId}", id);
            return StatusCode(500, ApiResponseDto<bool>.ErrorResult("Có lỗi xảy ra khi xóa giao dịch", 500));
        }
    }
}
