using AutoMapper;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.SystemAdmin.Application.DTOs;

namespace ManagementSystem.Modules.SystemAdmin.Application.Services;

public class BackupLogService : IBackupLogService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BackupLogService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginatedResultDto<BackupLogDto>> GetPagedAsync(BackupLogQueryParams query)
    {
        var (items, totalCount) = await _unitOfWork.BackupLogs.GetPagedAsync(query);
        return new PaginatedResultDto<BackupLogDto>
        {
            Items = _mapper.Map<List<BackupLogDto>>(items),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<BackupLogDto?> GetByIdAsync(Guid id)
    {
        var log = await _unitOfWork.BackupLogs.GetByIdAsync(id);
        return log is null ? null : _mapper.Map<BackupLogDto>(log);
    }
}
