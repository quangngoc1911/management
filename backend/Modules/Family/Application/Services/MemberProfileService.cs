using AutoMapper;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Modules.Family.Application.DTOs;
using ManagementSystem.Modules.Family.Domain.Entities;

namespace ManagementSystem.Modules.Family.Application.Services;

public class MemberProfileService : IMemberProfileService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;

    public MemberProfileService(IUnitOfWork unitOfWork, IMapper mapper, IDateTime dateTime)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dateTime = dateTime;
    }

    public async Task<MemberProfileDto?> GetByMemberIdAsync(Guid memberId)
    {
        if (!await _unitOfWork.FamilyMembers.ExistsAsync(memberId))
        {
            return null;
        }

        var profile = await _unitOfWork.MemberProfiles.GetByMemberIdAsync(memberId);
        return profile is null ? null : _mapper.Map<MemberProfileDto>(profile);
    }

    public async Task<MemberProfileDto?> UpsertAsync(Guid memberId, UpsertMemberProfileDto dto)
    {
        if (!await _unitOfWork.FamilyMembers.ExistsAsync(memberId))
        {
            return null;
        }

        var profile = await _unitOfWork.MemberProfiles.GetByMemberIdAsync(memberId);

        if (profile is null)
        {
            profile = _mapper.Map<MemberProfile>(dto);
            profile.MemberId = memberId;
            profile.CreatedAt = _dateTime.UtcNow;
            await _unitOfWork.MemberProfiles.CreateAsync(profile);
        }
        else
        {
            _mapper.Map(dto, profile);
            profile.UpdatedAt = _dateTime.UtcNow;
            _unitOfWork.MemberProfiles.Update(profile);
        }

        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<MemberProfileDto>(profile);
    }
}
