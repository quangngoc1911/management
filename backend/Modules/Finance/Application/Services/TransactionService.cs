using AutoMapper;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Finance.Application.DTOs;
using ManagementSystem.Modules.Finance.Domain.Entities;

namespace ManagementSystem.Modules.Finance.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;

    public TransactionService(IUnitOfWork unitOfWork, IMapper mapper, IDateTime dateTime)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dateTime = dateTime;
    }

    public async Task<PaginatedResultDto<TransactionDto>> GetPagedAsync(TransactionQueryParams query)
    {
        var (items, totalCount) = await _unitOfWork.Transactions.GetPagedAsync(query);
        return new PaginatedResultDto<TransactionDto>
        {
            Items = _mapper.Map<List<TransactionDto>>(items),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<TransactionDto?> GetByIdAsync(Guid id)
    {
        var transaction = await _unitOfWork.Transactions.GetByIdAsync(id);
        return transaction is null ? null : _mapper.Map<TransactionDto>(transaction);
    }

    public async Task<TransactionDto?> CreateAsync(CreateTransactionDto dto)
    {
        if (!await _unitOfWork.Accounts.ExistsAsync(dto.AccountId))
        {
            return null;
        }

        var transaction = _mapper.Map<Transaction>(dto);
        transaction.CreatedAt = _dateTime.UtcNow;

        await _unitOfWork.Transactions.CreateAsync(transaction);
        await _unitOfWork.SaveChangesAsync();

        var created = await _unitOfWork.Transactions.GetByIdAsync(transaction.Id);
        return _mapper.Map<TransactionDto>(created);
    }

    public async Task<TransactionDto?> UpdateAsync(Guid id, UpdateTransactionDto dto)
    {
        var transaction = await _unitOfWork.Transactions.GetForUpdateAsync(id);
        if (transaction is null)
        {
            return null;
        }

        if (!await _unitOfWork.Accounts.ExistsAsync(dto.AccountId))
        {
            return null;
        }

        _mapper.Map(dto, transaction);
        transaction.UpdatedAt = _dateTime.UtcNow;
        await _unitOfWork.SaveChangesAsync();

        var updated = await _unitOfWork.Transactions.GetByIdAsync(id);
        return _mapper.Map<TransactionDto>(updated);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleted = await _unitOfWork.Transactions.DeleteAsync(id);
        if (!deleted)
        {
            return false;
        }

        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
