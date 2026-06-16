using AutoMapper;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Finance.Application.DTOs;
using ManagementSystem.Modules.Finance.Domain.Entities;

namespace ManagementSystem.Modules.Finance.Application.Services;

public class AccountService : IAccountService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;

    public AccountService(IUnitOfWork unitOfWork, IMapper mapper, IDateTime dateTime)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dateTime = dateTime;
    }

    public async Task<PaginatedResultDto<AccountDto>> GetPagedAsync(AccountQueryParams query)
    {
        var (items, totalCount) = await _unitOfWork.Accounts.GetPagedAsync(query);
        return new PaginatedResultDto<AccountDto>
        {
            Items = _mapper.Map<List<AccountDto>>(items),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<AccountDto?> GetByIdAsync(Guid id)
    {
        var account = await _unitOfWork.Accounts.GetByIdAsync(id);
        return account is null ? null : _mapper.Map<AccountDto>(account);
    }

    public async Task<AccountDto> CreateAsync(CreateAccountDto dto)
    {
        var account = _mapper.Map<Account>(dto);
        account.CreatedAt = _dateTime.UtcNow;

        await _unitOfWork.Accounts.CreateAsync(account);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<AccountDto>(account);
    }

    public async Task<AccountDto?> UpdateAsync(Guid id, UpdateAccountDto dto)
    {
        var account = await _unitOfWork.Accounts.GetByIdAsync(id);
        if (account is null)
        {
            return null;
        }

        _mapper.Map(dto, account);
        account.UpdatedAt = _dateTime.UtcNow;

        _unitOfWork.Accounts.Update(account);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<AccountDto>(account);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleted = await _unitOfWork.Accounts.DeleteAsync(id);
        if (!deleted)
        {
            return false;
        }

        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
