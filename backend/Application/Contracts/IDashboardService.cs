using ManagementSystem.Application.DTOs.Common;

namespace ManagementSystem.Application.Contracts;

/// <summary>
/// Read-side service providing aggregate dashboard statistics.
/// </summary>
public interface IDashboardService
{
    Task<DashboardOverviewDto> GetOverviewAsync();
}
