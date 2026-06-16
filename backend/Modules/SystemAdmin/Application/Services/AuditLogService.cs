using AutoMapper;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.SystemAdmin.Application.DTOs;

namespace ManagementSystem.Modules.SystemAdmin.Application.Services;

public class AuditLogService : IAuditLogService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AuditLogService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginatedResultDto<AuditLogDto>> GetPagedAsync(AuditLogQueryParams query)
    {
        var (items, totalCount) = await _unitOfWork.AuditLogs.GetPagedAsync(query);
        return new PaginatedResultDto<AuditLogDto>
        {
            Items = _mapper.Map<List<AuditLogDto>>(items),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<AuditLogDto?> GetByIdAsync(Guid id)
    {
        var log = await _unitOfWork.AuditLogs.GetByIdAsync(id);
        return log is null ? null : _mapper.Map<AuditLogDto>(log);
    }
}
