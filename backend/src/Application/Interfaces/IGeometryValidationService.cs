using NetTopologySuite.Geometries;
using PugPlatform.Application.DTOs.GeoJson;

namespace PugPlatform.Application.Interfaces;

public interface IGeometryValidationService
{
    /// <summary>
    /// Validates geometry from GeoJSON
    /// </summary>
    GeometryValidationResult ValidateGeometry(
        GeoJsonFeature feature,
        GeometryType expectedType,
        int maxVertices = 5000);

    /// <summary>
    /// Validates polygon (no self-intersection, valid topology)
    /// </summary>
    GeometryValidationResult ValidatePolygon(Polygon polygon);

    /// <summary>
    /// Converts GeoJSON to NetTopologySuite geometry
    /// </summary>
    Geometry? ConvertFromGeoJson(GeoJsonFeature feature);

    /// <summary>
    /// Converts NetTopologySuite geometry to GeoJSON
    /// </summary>
    GeoJsonFeature? ConvertToGeoJson(Geometry geometry, Dictionary<string, object>? properties = null);
}

public enum GeometryType
{
    Point,
    Polygon,
    Any
}

public class GeometryValidationResult
{
    public bool IsValid { get; set; }
    public string? ErrorMessage { get; set; }
    public int VertexCount { get; set; }
    public bool HasSelfIntersection { get; set; }
}
