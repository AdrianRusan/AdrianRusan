using PugPlatform.Application.DTOs.GeoJson;

namespace PugPlatform.Application.DTOs.Scans;

public class CreateScanRequestDto
{
    public Guid? ParcelId { get; set; }
    public GeoJsonFeature? Geom { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Priority { get; set; } = "Normal";
    public string? ContactInfo { get; set; }
}

public class ScanRequestDto
{
    public Guid Id { get; set; }
    public Guid ApplicantId { get; set; }
    public Guid? ParcelId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public GeoJsonFeature? Geom { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public List<ScanFileDto> Files { get; set; } = new();
}

public class ScanFileDto
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public long Size { get; set; }
    public DateTime UploadedAt { get; set; }
    public string? DownloadUrl { get; set; }
}
