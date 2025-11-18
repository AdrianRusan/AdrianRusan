using PugPlatform.Application.DTOs.Scans;
using PugPlatform.Application.DTOs.Common;

namespace PugPlatform.Application.Interfaces;

public interface I3DScanService
{
    Task<ScanRequestDto> CreateScanRequestAsync(
        CreateScanRequestDto request,
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<ScanRequestDto?> GetScanRequestByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<PagedResult<ScanRequestDto>> GetScanRequestsAsync(
        Guid? parcelId,
        string? status,
        int page = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default);

    Task<List<ScanFileDto>> GetScansByParcelIdAsync(
        Guid parcelId,
        CancellationToken cancellationToken = default);

    Task<string> UploadScanFileAsync(
        Guid scanRequestId,
        Stream fileStream,
        string fileName,
        string contentType,
        long size,
        CancellationToken cancellationToken = default);

    Task<string> GetScanFileDownloadUrlAsync(
        Guid scanRequestId,
        Guid fileId,
        Guid? userId,
        CancellationToken cancellationToken = default);

    Task CompleteScanRequestAsync(
        Guid scanRequestId,
        bool notifyUser = true,
        CancellationToken cancellationToken = default);
}
