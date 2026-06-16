using ManagementSystem.Domain.Enums;

namespace ManagementSystem.Modules.Family.Application.DTOs;

/// <summary>
/// Query parameters for listing family members (search, filter, sort, paging).
/// </summary>
public class FamilyMemberQueryParams
{
    /// <summary>Free-text search over full name, nickname, phone and email.</summary>
    public string? Search { get; set; }

    public Gender? Gender { get; set; }
    public RelationToHead? RelationToHead { get; set; }
    public bool? IsHouseholdHead { get; set; }

    /// <summary>Sort field: "fullName" (default), "dateOfBirth" or "createdAt".</summary>
    public string SortBy { get; set; } = "fullName";
    public bool IsDescending { get; set; } = false;

    public int Page { get; set; } = 1;

    private int _pageSize = 20;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 100 ? 100 : value < 1 ? 1 : value;
    }
}
