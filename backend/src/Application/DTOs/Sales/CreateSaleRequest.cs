using PugPlatform.Application.DTOs.GeoJson;

namespace PugPlatform.Application.DTOs.Sales;

public class CreateSaleRequest
{
    public Guid? ParcelId { get; set; }
    public GeoJsonFeature? Geom { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = "RON";
    public string Description { get; set; } = string.Empty;
}

public class SaleDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? ParcelId { get; set; }
    public GeoJsonFeature? Geom { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsActive { get; set; }
    public List<string> PhotoUrls { get; set; } = new();
}
