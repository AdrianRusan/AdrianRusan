using PugPlatform.Application.DTOs.GeoJson;
using PugPlatform.Application.DTOs.Parcels;

namespace PugPlatform.Application.Interfaces;

public interface IParcelService
{
    Task<ParcelDto?> GetParcelByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ParcelDto?> GetParcelByCadastralIdAsync(string parcelId, CancellationToken cancellationToken = default);
    Task<GeoJsonFeatureCollection> GetParcelsByBboxAsync(
        double minLng,
        double minLat,
        double maxLng,
        double maxLat,
        string? query = null,
        CancellationToken cancellationToken = default);
}
