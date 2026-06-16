using Microsoft.EntityFrameworkCore;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Infrastructure.Persistence;
using ManagementSystem.Modules.Education.Application.DTOs;
using ManagementSystem.Modules.Education.Domain.Entities;

namespace ManagementSystem.Modules.Education.Infrastructure.Repositories;

public class StudyScheduleRepository : IStudyScheduleRepository
{
    private readonly ApplicationDbContext _context;

    public StudyScheduleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(IReadOnlyList<StudySchedule> Items, int TotalCount)> GetPagedAsync(StudyScheduleQueryParams query)
    {
        var q = _context.StudySchedules.AsNoTracking().Include(s => s.Member).AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower().Trim();
            q = q.Where(s => s.Title.ToLower().Contains(search) || (s.Subject != null && s.Subject.ToLower().Contains(search)));
        }
        if (query.MemberId.HasValue)
            q = q.Where(s => s.MemberId == query.MemberId);
        if (query.Status.HasValue)
            q = q.Where(s => s.Status == query.Status);
        if (query.FromDate.HasValue)
            q = q.Where(s => s.StartTime >= query.FromDate.Value);
        if (query.ToDate.HasValue)
            q = q.Where(s => s.StartTime <= query.ToDate.Value);

        q = query.SortBy?.ToLower() switch
        {
            "title" => query.IsDescending ? q.OrderByDescending(s => s.Title) : q.OrderBy(s => s.Title),
            _ => query.IsDescending ? q.OrderByDescending(s => s.StartTime) : q.OrderBy(s => s.StartTime)
        };

        var totalCount = await q.CountAsync();
        var items = await q.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToListAsync();
        return (items, totalCount);
    }

    public async Task<StudySchedule?> GetByIdAsync(Guid id) =>
        await _context.StudySchedules.AsNoTracking().Include(s => s.Member).FirstOrDefaultAsync(s => s.Id == id);

    public async Task<StudySchedule?> GetForUpdateAsync(Guid id) =>
        await _context.StudySchedules.FirstOrDefaultAsync(s => s.Id == id);

    public async Task CreateAsync(StudySchedule schedule) => await _context.StudySchedules.AddAsync(schedule);

    public async Task<bool> DeleteAsync(Guid id)
    {
        var schedule = await _context.StudySchedules.FirstOrDefaultAsync(s => s.Id == id);
        if (schedule == null) return false;
        schedule.IsDeleted = true;
        return true;
    }
}
