using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PugPlatform.Application.DTOs.Applications;
using PugPlatform.Application.Interfaces;
using System.Security.Claims;

namespace PugPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ApplicationsController : ControllerBase
{
    private readonly IApplicationService _applicationService;
    private readonly ILogger<ApplicationsController> _logger;

    public ApplicationsController(IApplicationService applicationService, ILogger<ApplicationsController> logger)
    {
        _applicationService = applicationService;
        _logger = logger;
    }

    /// <summary>
    /// Create a new application
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApplicationDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateApplicationRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = GetUserId();
            var application = await _applicationService.CreateApplicationAsync(request, userId, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = application.Id }, application);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating application");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get application by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApplicationDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var application = await _applicationService.GetApplicationByIdAsync(id, cancellationToken);
        if (application == null)
        {
            return NotFound(new { message = "Application not found" });
        }

        return Ok(application);
    }

    /// <summary>
    /// Get applications with filtering and pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<ApplicationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? status,
        [FromQuery] string? type,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var applications = await _applicationService.GetApplicationsAsync(
            status, type, page, pageSize, cancellationToken);

        return Ok(applications);
    }

    /// <summary>
    /// Transition application status (staff only)
    /// </summary>
    [HttpPost("{id}/transition")]
    [Authorize(Roles = "UrbanismStaff,Admin,SuperAdmin")]
    [ProducesResponseType(typeof(ApplicationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> TransitionStatus(
        Guid id,
        [FromBody] TransitionStatusRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = GetUserId();
            var application = await _applicationService.TransitionStatusAsync(
                id, request.NewStatus, request.Comment, userId, cancellationToken);

            return Ok(application);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error transitioning application status");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Upload attachment to application
    /// </summary>
    [HttpPost("{id}/attachments")]
    [ProducesResponseType(typeof(AttachmentUploadResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> UploadAttachment(
        Guid id,
        [FromForm] IFormFile file,
        CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { message = "No file uploaded" });
        }

        try
        {
            using var stream = file.OpenReadStream();
            var storagePath = await _applicationService.UploadAttachmentAsync(
                id, stream, file.FileName, file.ContentType, file.Length, cancellationToken);

            return Ok(new AttachmentUploadResponse
            {
                FileName = file.FileName,
                StoragePath = storagePath,
                Size = file.Length
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading attachment");
            return BadRequest(new { message = ex.Message });
        }
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim ?? throw new UnauthorizedAccessException("User ID not found"));
    }
}

public class TransitionStatusRequest
{
    public string NewStatus { get; set; } = string.Empty;
    public string? Comment { get; set; }
}

public class AttachmentUploadResponse
{
    public string FileName { get; set; } = string.Empty;
    public string StoragePath { get; set; } = string.Empty;
    public long Size { get; set; }
}
