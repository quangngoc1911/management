namespace ManagementSystem.Application.DTOs.Common;

/// <summary>
/// Aggregate counts shown on the dashboard overview.
/// </summary>
public class DashboardOverviewDto
{
    public int TotalUsers { get; set; }
    public int TotalDocuments { get; set; }
    public int TotalCategories { get; set; }
    public int TotalTags { get; set; }
    public int TotalFamilyMembers { get; set; }
}
