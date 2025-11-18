namespace PugPlatform.Application.DTOs.Issues;

public class IssueStatsDto
{
    public int TotalIssues { get; set; }
    public int NewIssues { get; set; }
    public int InProgressIssues { get; set; }
    public int ResolvedIssues { get; set; }
    public int RejectedIssues { get; set; }
    public Dictionary<string, int> IssuesByCategory { get; set; } = new();
    public Dictionary<string, int> IssuesByMonth { get; set; } = new();
    public double AverageResolutionTimeHours { get; set; }
}

public class ReassignIssueRequest
{
    public string Category { get; set; } = string.Empty;
    public Guid? AssignToUserId { get; set; }
}
