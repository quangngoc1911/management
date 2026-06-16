using Microsoft.EntityFrameworkCore;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Infrastructure.Persistence;

namespace ManagementSystem.Infrastructure.Services;

/// <summary>
/// Computes dashboard aggregate counts directly from the read model.
/// Soft-deleted rows are excluded by the global query filter.
/// </summary>
public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _context;

    public DashboardService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardOverviewDto> GetOverviewAsync()
    {
        return new DashboardOverviewDto
        {
            TotalUsers = await _context.Users.CountAsync(),
            TotalDocuments = await _context.Documents.CountAsync(),
            TotalCategories = await _context.Categories.CountAsync(),
            TotalTags = await _context.Tags.CountAsync(),
            TotalFamilyMembers = await _context.FamilyMembers.CountAsync()
        };
    }
}
