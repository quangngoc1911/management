using ManagementSystem.Application.DTOs.Common;
using ManagementSystem.Modules.Family.Application.DTOs;

namespace ManagementSystem.Application.Contracts;

/// <summary>
/// Application service for managing family members.
/// </summary>
public interface IFamilyMemberService
{
    Task<PaginatedResultDto<FamilyMemberDto>> GetPagedAsync(FamilyMemberQueryParams query);
    Task<FamilyMemberDto?> GetByIdAsync(Guid id);
    Task<FamilyMemberDto> CreateAsync(CreateFamilyMemberDto dto);
    Task<FamilyMemberDto?> UpdateAsync(Guid id, UpdateFamilyMemberDto dto);
    Task<bool> DeleteAsync(Guid id);
}
