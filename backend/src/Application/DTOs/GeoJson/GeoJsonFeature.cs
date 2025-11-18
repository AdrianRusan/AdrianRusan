using System.Text.Json.Serialization;

namespace PugPlatform.Application.DTOs.GeoJson;

public class GeoJsonFeature
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "Feature";

    [JsonPropertyName("geometry")]
    public GeoJsonGeometry? Geometry { get; set; }

    [JsonPropertyName("properties")]
    public Dictionary<string, object>? Properties { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }
}

public class GeoJsonGeometry
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("coordinates")]
    public object? Coordinates { get; set; }
}

public class GeoJsonFeatureCollection
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "FeatureCollection";

    [JsonPropertyName("features")]
    public List<GeoJsonFeature> Features { get; set; } = new();

    [JsonPropertyName("crs")]
    public GeoJsonCrs? Crs { get; set; }
}

public class GeoJsonCrs
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "name";

    [JsonPropertyName("properties")]
    public Dictionary<string, string> Properties { get; set; } = new()
    {
        { "name", "EPSG:4326" }
    };
}
