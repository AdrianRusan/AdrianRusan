using NetTopologySuite.Geometries;
using PugPlatform.Domain.Enums;

namespace PugPlatform.Domain.Entities;

public class ScanRequest
{
    public Guid Id { get; set; }
    public Guid ApplicantId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Point? Geom { get; set; }
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Submitted;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }

    // Navigation properties
    public User Applicant { get; set; } = null!;
    public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
}
