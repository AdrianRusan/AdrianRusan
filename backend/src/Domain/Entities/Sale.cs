using NetTopologySuite.Geometries;
using PugPlatform.Domain.Enums;

namespace PugPlatform.Domain.Entities;

public class Sale
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? ParcelId { get; set; }
    public Polygon? Geom { get; set; } // Used if parcel not in DB
    public decimal Price { get; set; }
    public Currency Currency { get; set; } = Currency.RON;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; }
    public bool IsActive { get; set; } = true;
    public bool RenewalReminderSent { get; set; } = false;

    // Navigation properties
    public User User { get; set; } = null!;
    public Parcel? Parcel { get; set; }
    public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
}
