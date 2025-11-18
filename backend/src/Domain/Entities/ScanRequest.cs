using NetTopologySuite.Geometries;
using PugPlatform.Domain.Enums;

namespace PugPlatform.Domain.Entities;

public class ScanRequest
{
    public Guid Id { get; set; }
    public Guid ApplicantId { get; set; }
    public Guid? ParcelId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Point? Geom { get; set; }
    public ScanPriority Priority { get; set; } = ScanPriority.Normal;
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Submitted;
    public bool IsPublic { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public string? ContactInfo { get; set; }

    // Navigation properties
    public User Applicant { get; set; } = null!;
    public Parcel? Parcel { get; set; }
    public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
}
