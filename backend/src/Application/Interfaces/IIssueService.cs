using PugPlatform.Application.DTOs.Issues;
using PugPlatform.Application.DTOs.Common;

namespace PugPlatform.Application.Interfaces;

public interface IIssueService
{
    Task<IssueDto> CreateIssueAsync(
        CreateIssueRequest request,
        CancellationToken cancellationToken = default);

    Task<IssueDto?> GetIssueByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<PagedResult<IssueDto>> GetIssuesAsync(
        string? status,
        double? minLng,
        double? minLat,
        double? maxLng,
        double? maxLat,
        int page = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default);

    Task RespondToIssueAsync(
        Guid id,
        string response,
        string newStatus,
        CancellationToken cancellationToken = default);

    Task<string> UploadPhotoAsync(
        Guid issueId,
        Stream fileStream,
        string fileName,
        string contentType,
        long size,
        CancellationToken cancellationToken = default);
}
