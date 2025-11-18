using NetTopologySuite.Geometries;

namespace PugPlatform.Domain.Entities;

public class ConstructionItem
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public Polygon? Geom { get; set; }
    public string BuildingType { get; set; } = string.Empty;
    public decimal? PlannedArea { get; set; }
    public int? Floors { get; set; }
    public string? Description { get; set; }

    // Navigation properties
    public Application Application { get; set; } = null!;
}
