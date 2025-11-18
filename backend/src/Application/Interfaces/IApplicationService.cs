using PugPlatform.Application.DTOs.Applications;
using PugPlatform.Application.DTOs.Common;

namespace PugPlatform.Application.Interfaces;

public interface IApplicationService
{
    Task<ApplicationDto> CreateApplicationAsync(
        CreateApplicationRequest request,
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<ApplicationDetailDto?> GetApplicationByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<PagedResult<ApplicationDto>> GetApplicationsAsync(
        string? status,
        string? type,
        int page = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default);

    Task<ApplicationDto> TransitionStatusAsync(
        Guid id,
        string newStatus,
        string? comment,
        Guid actorId,
        CancellationToken cancellationToken = default);

    Task<string> UploadAttachmentAsync(
        Guid applicationId,
        Stream fileStream,
        string fileName,
        string contentType,
        long size,
        CancellationToken cancellationToken = default);
}
