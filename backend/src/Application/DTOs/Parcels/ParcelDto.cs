using PugPlatform.Application.DTOs.GeoJson;

namespace PugPlatform.Application.DTOs.Parcels;

public class ParcelDto
{
    public Guid Id { get; set; }
    public string ParcelId { get; set; } = string.Empty;
    public decimal SurfaceM2 { get; set; }
    public decimal SurfaceHa { get; set; }
    public string PlotType { get; set; } = string.Empty;
    public GeoJsonFeature? Geometry { get; set; }
    public Dictionary<string, object>? Properties { get; set; }
}
