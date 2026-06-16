using AutoMapper;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Finance.Application.DTOs;
using ManagementSystem.Modules.Finance.Domain.Entities;

namespace ManagementSystem.Modules.Finance.Application.Services;

public class RecurringTransactionService : IRecurringTransactionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;

    public RecurringTransactionService(IUnitOfWork unitOfWork, IMapper mapper, IDateTime dateTime)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dateTime = dateTime;
    }

    public async Task<PaginatedResultDto<RecurringTransactionDto>> GetPagedAsync(RecurringTransactionQueryParams query)
    {
        var (items, totalCount) = await _unitOfWork.RecurringTransactions.GetPagedAsync(query);
        return new PaginatedResultDto<RecurringTransactionDto>
        {
            Items = _mapper.Map<List<RecurringTransactionDto>>(items),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<RecurringTransactionDto?> GetByIdAsync(Guid id)
    {
        var recurring = await _unitOfWork.RecurringTransactions.GetByIdAsync(id);
        return recurring is null ? null : _mapper.Map<RecurringTransactionDto>(recurring);
    }

    public async Task<RecurringTransactionDto?> CreateAsync(CreateRecurringTransactionDto dto)
    {
        if (!await _unitOfWork.Accounts.ExistsAsync(dto.AccountId))
        {
            return null;
        }

        var recurring = _mapper.Map<RecurringTransaction>(dto);
        recurring.CreatedAt = _dateTime.UtcNow;
        await _unitOfWork.RecurringTransactions.CreateAsync(recurring);
        await _unitOfWork.SaveChangesAsync();
        var created = await _unitOfWork.RecurringTransactions.GetByIdAsync(recurring.Id);
        return _mapper.Map<RecurringTransactionDto>(created);
    }

    public async Task<RecurringTransactionDto?> UpdateAsync(Guid id, UpdateRecurringTransactionDto dto)
    {
        var recurring = await _unitOfWork.RecurringTransactions.GetForUpdateAsync(id);
        if (recurring is null) return null;
        if (!await _unitOfWork.Accounts.ExistsAsync(dto.AccountId)) return null;

        _mapper.Map(dto, recurring);
        recurring.UpdatedAt = _dateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();
        var updated = await _unitOfWork.RecurringTransactions.GetByIdAsync(id);
        return _mapper.Map<RecurringTransactionDto>(updated);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleted = await _unitOfWork.RecurringTransactions.DeleteAsync(id);
        if (!deleted) return false;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
