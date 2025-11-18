using NetTopologySuite.Geometries;
using PugPlatform.Domain.Enums;

namespace PugPlatform.Domain.Entities;

public class Issue
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Point? Geom { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public IssueStatus Status { get; set; } = IssueStatus.New;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? Response { get; set; }
    public DateTime? RespondedAt { get; set; }

    // Navigation properties
    public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
}
