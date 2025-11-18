using PugPlatform.Application.DTOs.GeoJson;

namespace PugPlatform.Application.DTOs.Applications;

public class ApplicationDto
{
    public Guid Id { get; set; }
    public string ApplicationType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string TrackingNumber { get; set; } = string.Empty;
    public GeoJsonFeature? Geom { get; set; }
    public Dictionary<string, object>? Data { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<string> AttachmentUrls { get; set; } = new();
}

public class ApplicationDetailDto : ApplicationDto
{
    public ApplicantDto? Applicant { get; set; }
    public List<CommentDto> Comments { get; set; } = new();
}

public class ApplicantDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Phone { get; set; }
}

public class CommentDto
{
    public Guid Id { get; set; }
    public string Comment { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public bool IsInternal { get; set; }
    public DateTime CreatedAt { get; set; }
}
