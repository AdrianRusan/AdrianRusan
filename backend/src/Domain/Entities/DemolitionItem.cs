using NetTopologySuite.Geometries;

namespace PugPlatform.Domain.Entities;

public class DemolitionItem
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public Polygon? Geom { get; set; }
    public string StructureType { get; set; } = string.Empty;
    public int? YearConstruction { get; set; }
    public string? Material { get; set; }
    public string Motivation { get; set; } = string.Empty;

    // Navigation properties
    public Application Application { get; set; } = null!;
}
