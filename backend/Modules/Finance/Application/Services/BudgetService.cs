using AutoMapper;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Finance.Application.DTOs;
using ManagementSystem.Modules.Finance.Domain.Entities;

namespace ManagementSystem.Modules.Finance.Application.Services;

public class BudgetService : IBudgetService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;

    public BudgetService(IUnitOfWork unitOfWork, IMapper mapper, IDateTime dateTime)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dateTime = dateTime;
    }

    public async Task<PaginatedResultDto<BudgetDto>> GetPagedAsync(BudgetQueryParams query)
    {
        var (items, totalCount) = await _unitOfWork.Budgets.GetPagedAsync(query);
        return new PaginatedResultDto<BudgetDto>
        {
            Items = _mapper.Map<List<BudgetDto>>(items),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<BudgetDto?> GetByIdAsync(Guid id)
    {
        var budget = await _unitOfWork.Budgets.GetByIdAsync(id);
        return budget is null ? null : _mapper.Map<BudgetDto>(budget);
    }

    public async Task<BudgetDto> CreateAsync(CreateBudgetDto dto)
    {
        var budget = _mapper.Map<Budget>(dto);
        budget.CreatedAt = _dateTime.UtcNow;
        await _unitOfWork.Budgets.CreateAsync(budget);
        await _unitOfWork.SaveChangesAsync();
        var created = await _unitOfWork.Budgets.GetByIdAsync(budget.Id);
        return _mapper.Map<BudgetDto>(created);
    }

    public async Task<BudgetDto?> UpdateAsync(Guid id, UpdateBudgetDto dto)
    {
        var budget = await _unitOfWork.Budgets.GetForUpdateAsync(id);
        if (budget is null) return null;
        _mapper.Map(dto, budget);
        budget.UpdatedAt = _dateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
        var updated = await _unitOfWork.Budgets.GetByIdAsync(id);
        return _mapper.Map<BudgetDto>(updated);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleted = await _unitOfWork.Budgets.DeleteAsync(id);
        if (!deleted) return false;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
