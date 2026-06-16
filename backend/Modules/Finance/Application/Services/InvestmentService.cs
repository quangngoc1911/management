using AutoMapper;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Finance.Application.DTOs;
using ManagementSystem.Modules.Finance.Domain.Entities;

namespace ManagementSystem.Modules.Finance.Application.Services;

public class InvestmentService : IInvestmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;

    public InvestmentService(IUnitOfWork unitOfWork, IMapper mapper, IDateTime dateTime)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dateTime = dateTime;
    }

    public async Task<PaginatedResultDto<InvestmentDto>> GetPagedAsync(InvestmentQueryParams query)
    {
        var (items, totalCount) = await _unitOfWork.Investments.GetPagedAsync(query);
        return new PaginatedResultDto<InvestmentDto>
        {
            Items = _mapper.Map<List<InvestmentDto>>(items),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<InvestmentDto?> GetByIdAsync(Guid id)
    {
        var investment = await _unitOfWork.Investments.GetByIdAsync(id);
        return investment is null ? null : _mapper.Map<InvestmentDto>(investment);
    }

    public async Task<InvestmentDto> CreateAsync(CreateInvestmentDto dto)
    {
        var investment = _mapper.Map<Investment>(dto);
        investment.CreatedAt = _dateTime.UtcNow;
        await _unitOfWork.Investments.CreateAsync(investment);
        await _unitOfWork.SaveChangesAsync();
        var created = await _unitOfWork.Investments.GetByIdAsync(investment.Id);
        return _mapper.Map<InvestmentDto>(created);
    }

    public async Task<InvestmentDto?> UpdateAsync(Guid id, UpdateInvestmentDto dto)
    {
        var investment = await _unitOfWork.Investments.GetForUpdateAsync(id);
        if (investment is null) return null;
        _mapper.Map(dto, investment);
        investment.UpdatedAt = _dateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
        var updated = await _unitOfWork.Investments.GetByIdAsync(id);
        return _mapper.Map<InvestmentDto>(updated);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleted = await _unitOfWork.Investments.DeleteAsync(id);
        if (!deleted) return false;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
