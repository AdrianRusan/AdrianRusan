using NetTopologySuite.Geometries;
using PugPlatform.Domain.Enums;

namespace PugPlatform.Domain.Entities;

public class Application
{
    public Guid Id { get; set; }
    public ApplicationType ApplicationType { get; set; }
    public Guid ApplicantId { get; set; }
    public Geometry? Geom { get; set; }
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Draft;
    public string TrackingNumber { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string? Data { get; set; } // JSONB for domain-specific fields

    // Navigation properties
    public User Applicant { get; set; } = null!;
    public ICollection<TreeItem> TreeItems { get; set; } = new List<TreeItem>();
    public ICollection<DemolitionItem> DemolitionItems { get; set; } = new List<DemolitionItem>();
    public ICollection<ConstructionItem> ConstructionItems { get; set; } = new List<ConstructionItem>();
    public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    public ICollection<ApplicationComment> Comments { get; set; } = new List<ApplicationComment>();
}
