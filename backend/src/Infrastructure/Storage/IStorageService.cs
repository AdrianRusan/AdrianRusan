namespace PugPlatform.Infrastructure.Storage;

public interface IStorageService
{
    Task<string> UploadFileAsync(
        string bucketName,
        string objectName,
        Stream data,
        long size,
        string contentType,
        CancellationToken cancellationToken = default);

    Task<string> GetPresignedUrlAsync(
        string bucketName,
        string objectName,
        int expiryInSeconds = 3600,
        CancellationToken cancellationToken = default);

    Task DeleteFileAsync(
        string bucketName,
        string objectName,
        CancellationToken cancellationToken = default);

    Task<bool> BucketExistsAsync(
        string bucketName,
        CancellationToken cancellationToken = default);

    Task CreateBucketAsync(
        string bucketName,
        CancellationToken cancellationToken = default);
}
