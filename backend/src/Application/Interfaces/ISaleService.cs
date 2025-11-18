using PugPlatform.Application.DTOs.Sales;
using PugPlatform.Application.DTOs.Common;

namespace PugPlatform.Application.Interfaces;

public interface ISaleService
{
    Task<SaleDto> CreateSaleAsync(
        CreateSaleRequest request,
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<SaleDto?> GetSaleByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<PagedResult<SaleDto>> GetActiveSalesAsync(
        double? minLng,
        double? minLat,
        double? maxLng,
        double? maxLat,
        int page = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default);

    Task<SaleDto> RenewSaleAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<string> UploadPhotoAsync(
        Guid saleId,
        Stream fileStream,
        string fileName,
        string contentType,
        long size,
        CancellationToken cancellationToken = default);
}
