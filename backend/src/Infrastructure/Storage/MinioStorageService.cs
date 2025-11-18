using Minio;
using Minio.DataModel.Args;

namespace PugPlatform.Infrastructure.Storage;

public class MinioStorageService : IStorageService
{
    private readonly IMinioClient _minioClient;

    public MinioStorageService(IMinioClient minioClient)
    {
        _minioClient = minioClient;
    }

    public async Task<string> UploadFileAsync(
        string bucketName,
        string objectName,
        Stream data,
        long size,
        string contentType,
        CancellationToken cancellationToken = default)
    {
        // Ensure bucket exists
        var bucketExists = await BucketExistsAsync(bucketName, cancellationToken);
        if (!bucketExists)
        {
            await CreateBucketAsync(bucketName, cancellationToken);
        }

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithStreamData(data)
            .WithObjectSize(size)
            .WithContentType(contentType);

        await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

        return objectName;
    }

    public async Task<string> GetPresignedUrlAsync(
        string bucketName,
        string objectName,
        int expiryInSeconds = 3600,
        CancellationToken cancellationToken = default)
    {
        var presignedGetObjectArgs = new PresignedGetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithExpiry(expiryInSeconds);

        return await _minioClient.PresignedGetObjectAsync(presignedGetObjectArgs);
    }

    public async Task DeleteFileAsync(
        string bucketName,
        string objectName,
        CancellationToken cancellationToken = default)
    {
        var removeObjectArgs = new RemoveObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName);

        await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);
    }

    public async Task<bool> BucketExistsAsync(
        string bucketName,
        CancellationToken cancellationToken = default)
    {
        var bucketExistsArgs = new BucketExistsArgs()
            .WithBucket(bucketName);

        return await _minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken);
    }

    public async Task CreateBucketAsync(
        string bucketName,
        CancellationToken cancellationToken = default)
    {
        var makeBucketArgs = new MakeBucketArgs()
            .WithBucket(bucketName);

        await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
    }
}
