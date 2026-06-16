using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;

namespace ManagementSystem.Modules.Dashboard.Presentation.Controllers;

[ApiController]
[Route("api/dashboard")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(IDashboardService dashboardService, ILogger<DashboardController> logger)
    {
        _dashboardService = dashboardService;
        _logger = logger;
    }

    [HttpGet("stats")]
    public async Task<ActionResult<ApiResponseDto<DashboardOverviewDto>>> GetStatsAsync()
    {
        try
        {
            var stats = await _dashboardService.GetOverviewAsync();
            return Ok(ApiResponseDto<DashboardOverviewDto>.SuccessResult(stats, "Lấy thống kê tổng quan thành công"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting dashboard stats");
            return StatusCode(500, ApiResponseDto<DashboardOverviewDto>.ErrorResult("Có lỗi xảy ra khi lấy thống kê", 500));
        }
    }
}
