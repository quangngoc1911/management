namespace ManagementSystem.Application.DTOs.Common;

public class DashboardStatsDto
{
    public int TotalDocuments { get; set; }
    public int ActiveDocuments { get; set; }
    public int ExpiredDocuments { get; set; }
    public int ExpiringSoonDocuments { get; set; }
    public int TotalCategories { get; set; }
    public int TotalUsers { get; set; }
    public List<StatusCountDto> DocumentsByStatus { get; set; } = new();
    public List<CategoryCountDto> DocumentsByCategory { get; set; } = new();
    public List<RecentActivityDto> RecentActivities { get; set; } = new();
}

public class StatusCountDto
{
    public string Status { get; set; } = string.Empty;
    public int Count { get; set; }
}

public class CategoryCountDto
{
    public string CategoryId { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public int Count { get; set; }
}

public class RecentActivityDto
{
    public Guid DocumentId { get; set; }
    public string DocumentTitle { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}