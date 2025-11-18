using NetTopologySuite.Geometries;
using PugPlatform.Domain.Enums;

namespace PugPlatform.Domain.Entities;

public class DemolitionItem
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public Polygon? Geom { get; set; }
    public StructureType StructureType { get; set; }
    public ConstructionMaterial Material { get; set; }
    public int? YearConstruction { get; set; }
    public int? ApproximateAge { get; set; }
    public string Motivation { get; set; } = string.Empty;

    // Navigation properties
    public Application Application { get; set; } = null!;
}
