using PugPlatform.Application.DTOs.GeoJson;

namespace PugPlatform.Application.DTOs.Applications;

public class CreateApplicationRequest
{
    public string ApplicationType { get; set; } = string.Empty;
    public GeoJsonFeature? Geom { get; set; }
    public Dictionary<string, object>? Data { get; set; }
}

public class CreateTreeCuttingRequest : CreateApplicationRequest
{
    public List<TreeItemDto>? Trees { get; set; }
}

public class TreeItemDto
{
    public GeoJsonFeature? Geom { get; set; }
    public string Species { get; set; } = string.Empty;
    public int? AgeEstimate { get; set; }
    public decimal? DiameterCm { get; set; }
    public string? Condition { get; set; }
    public string Motivation { get; set; } = string.Empty;
}

public class CreateDemolitionRequest : CreateApplicationRequest
{
    public List<DemolitionItemDto>? Items { get; set; }
}

public class DemolitionItemDto
{
    public GeoJsonFeature? Geom { get; set; }
    public string StructureType { get; set; } = string.Empty;
    public int? YearConstruction { get; set; }
    public string? Material { get; set; }
    public string Motivation { get; set; } = string.Empty;
}

public class CreateConstructionRequest : CreateApplicationRequest
{
    public List<ConstructionItemDto>? Items { get; set; }
}

public class ConstructionItemDto
{
    public GeoJsonFeature? Geom { get; set; }
    public string BuildingType { get; set; } = string.Empty;
    public decimal? PlannedArea { get; set; }
    public int? Floors { get; set; }
    public string? Description { get; set; }
}
