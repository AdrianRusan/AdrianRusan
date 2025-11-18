using NetTopologySuite.Geometries;

namespace PugPlatform.Domain.Entities;

public class TreeItem
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public Point? Geom { get; set; }
    public string Species { get; set; } = string.Empty;
    public int? AgeEstimate { get; set; }
    public decimal? DiameterCm { get; set; }
    public string? Condition { get; set; }
    public string Motivation { get; set; } = string.Empty;

    // Navigation properties
    public Application Application { get; set; } = null!;
}
