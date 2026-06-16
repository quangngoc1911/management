namespace ManagementSystem.Modules.Assets.Application.DTOs;

public class AssetValuationQueryParams
{
    public Guid? AssetId { get; set; }
    public string SortBy { get; set; } = "valuationdate";
    public bool IsDescending { get; set; } = true;
    public int Page { get; set; } = 1;

    private int _pageSize = 20;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 100 ? 100 : value < 1 ? 1 : value;
    }
}
