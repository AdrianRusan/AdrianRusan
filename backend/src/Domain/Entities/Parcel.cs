using NetTopologySuite.Geometries;
using PugPlatform.Domain.Enums;

namespace PugPlatform.Domain.Entities;

public class Parcel
{
    public Guid Id { get; set; }
    public string ParcelId { get; set; } = string.Empty;
    public Guid? OwnerId { get; set; }
    public Polygon? Geom { get; set; }
    public decimal SurfaceM2 { get; set; }
    public decimal SurfaceHa { get; set; }
    public PlotType PlotType { get; set; } = PlotType.Other;
    public string? Properties { get; set; } // JSONB
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public User? Owner { get; set; }
    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
    public ParcelTaxStatus? TaxStatus { get; set; }
}
