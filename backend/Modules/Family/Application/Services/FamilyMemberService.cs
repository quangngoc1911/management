using AutoMapper;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Family.Application.DTOs;
using ManagementSystem.Modules.Family.Domain.Entities;

namespace ManagementSystem.Modules.Family.Application.Services;

public class FamilyMemberService : IFamilyMemberService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;

    public FamilyMemberService(IUnitOfWork unitOfWork, IMapper mapper, IDateTime dateTime)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dateTime = dateTime;
    }

    public async Task<PaginatedResultDto<FamilyMemberDto>> GetPagedAsync(FamilyMemberQueryParams query)
    {
        var (items, totalCount) = await _unitOfWork.FamilyMembers.GetPagedAsync(query);

        return new PaginatedResultDto<FamilyMemberDto>
        {
            Items = _mapper.Map<List<FamilyMemberDto>>(items),
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<FamilyMemberDto?> GetByIdAsync(Guid id)
    {
        var member = await _unitOfWork.FamilyMembers.GetByIdAsync(id);
        return member is null ? null : _mapper.Map<FamilyMemberDto>(member);
    }

    public async Task<FamilyMemberDto> CreateAsync(CreateFamilyMemberDto dto)
    {
        var member = _mapper.Map<FamilyMember>(dto);
        member.CreatedAt = _dateTime.UtcNow;

        await _unitOfWork.FamilyMembers.CreateAsync(member);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<FamilyMemberDto>(member);
    }

    public async Task<FamilyMemberDto?> UpdateAsync(Guid id, UpdateFamilyMemberDto dto)
    {
        var member = await _unitOfWork.FamilyMembers.GetByIdAsync(id);
        if (member is null)
        {
            return null;
        }

        _mapper.Map(dto, member);
        member.UpdatedAt = _dateTime.UtcNow;

        _unitOfWork.FamilyMembers.Update(member);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<FamilyMemberDto>(member);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleted = await _unitOfWork.FamilyMembers.DeleteAsync(id);
        if (!deleted)
        {
            return false;
        }

        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
