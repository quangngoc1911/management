using AutoMapper;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Modules.Family.Application.DTOs;
using ManagementSystem.Modules.Family.Domain.Entities;

namespace ManagementSystem.Modules.Family.Application.Services;

public class MemberRelationshipService : IMemberRelationshipService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDateTime _dateTime;

    public MemberRelationshipService(IUnitOfWork unitOfWork, IMapper mapper, IDateTime dateTime)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _dateTime = dateTime;
    }

    public async Task<List<MemberRelationshipDto>?> GetByMemberAsync(Guid memberId)
    {
        if (!await _unitOfWork.FamilyMembers.ExistsAsync(memberId))
        {
            return null;
        }

        var relationships = await _unitOfWork.Relationships.GetByMemberIdAsync(memberId);
        return _mapper.Map<List<MemberRelationshipDto>>(relationships);
    }

    public async Task<MemberRelationshipDto?> CreateAsync(CreateMemberRelationshipDto dto)
    {
        if (!await _unitOfWork.FamilyMembers.ExistsAsync(dto.MemberId) ||
            !await _unitOfWork.FamilyMembers.ExistsAsync(dto.RelatedMemberId))
        {
            return null;
        }

        if (await _unitOfWork.Relationships.TripleExistsAsync(dto.MemberId, dto.RelatedMemberId, dto.RelationshipType))
        {
            throw new InvalidOperationException("Quan hệ này đã tồn tại giữa hai thành viên");
        }

        var relationship = _mapper.Map<MemberRelationship>(dto);
        relationship.CreatedAt = _dateTime.UtcNow;

        await _unitOfWork.Relationships.CreateAsync(relationship);
        await _unitOfWork.SaveChangesAsync();

        var created = await _unitOfWork.Relationships.GetByIdAsync(relationship.Id);
        return _mapper.Map<MemberRelationshipDto>(created);
    }

    public async Task<MemberRelationshipDto?> UpdateAsync(Guid id, UpdateMemberRelationshipDto dto)
    {
        var relationship = await _unitOfWork.Relationships.GetByIdAsync(id);
        if (relationship is null)
        {
            return null;
        }

        if (await _unitOfWork.Relationships.TripleExistsAsync(relationship.MemberId, relationship.RelatedMemberId, dto.RelationshipType, id))
        {
            throw new InvalidOperationException("Quan hệ này đã tồn tại giữa hai thành viên");
        }

        _mapper.Map(dto, relationship);
        relationship.UpdatedAt = _dateTime.UtcNow;

        _unitOfWork.Relationships.Update(relationship);
        await _unitOfWork.SaveChangesAsync();

        var updated = await _unitOfWork.Relationships.GetByIdAsync(id);
        return _mapper.Map<MemberRelationshipDto>(updated);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleted = await _unitOfWork.Relationships.DeleteAsync(id);
        if (!deleted)
        {
            return false;
        }

        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
