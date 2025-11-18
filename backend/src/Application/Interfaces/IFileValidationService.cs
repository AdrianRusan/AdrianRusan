namespace PugPlatform.Application.Interfaces;

public interface IFileValidationService
{
    /// <summary>
    /// Validates file size and type
    /// </summary>
    Task<FileValidationResult> ValidateFileAsync(
        Stream fileStream,
        string fileName,
        string contentType,
        long fileSize,
        FileUploadCategory category,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Resizes image if needed
    /// </summary>
    Task<Stream> ResizeImageIfNeededAsync(
        Stream imageStream,
        long maxSizeBytes = 5 * 1024 * 1024,
        CancellationToken cancellationToken = default);
}

public enum FileUploadCategory
{
    Photo,
    Document,
    ThreeDModel,
    GeoData
}

public class FileValidationResult
{
    public bool IsValid { get; set; }
    public string? ErrorMessage { get; set; }
    public long? ResizedSize { get; set; }
    public bool WasResized { get; set; }
}
