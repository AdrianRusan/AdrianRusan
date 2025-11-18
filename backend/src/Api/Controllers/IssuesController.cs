using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PugPlatform.Application.DTOs.Issues;
using PugPlatform.Application.Interfaces;

namespace PugPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IssuesController : ControllerBase
{
    private readonly IIssueService _issueService;
    private readonly ILogger<IssuesController> _logger;

    public IssuesController(IIssueService issueService, ILogger<IssuesController> logger)
    {
        _issueService = issueService;
        _logger = logger;
    }

    /// <summary>
    /// Create a new issue (public endpoint)
    /// </summary>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IssueDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateIssueRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var issue = await _issueService.CreateIssueAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = issue.Id }, issue);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating issue");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get issue by ID
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IssueDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var issue = await _issueService.GetIssueByIdAsync(id, cancellationToken);
        if (issue == null)
        {
            return NotFound(new { message = "Issue not found" });
        }

        return Ok(issue);
    }

    /// <summary>
    /// Get issues with filtering
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "UrbanismStaff,Admin,SuperAdmin")]
    [ProducesResponseType(typeof(PagedResult<IssueDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? status,
        [FromQuery] double? minLng,
        [FromQuery] double? minLat,
        [FromQuery] double? maxLng,
        [FromQuery] double? maxLat,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var issues = await _issueService.GetIssuesAsync(
            status, minLng, minLat, maxLng, maxLat, page, pageSize, cancellationToken);

        return Ok(issues);
    }

    /// <summary>
    /// Respond to issue (staff only)
    /// </summary>
    [HttpPost("{id}/respond")]
    [Authorize(Roles = "UrbanismStaff,Admin,SuperAdmin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Respond(
        Guid id,
        [FromBody] RespondToIssueRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            await _issueService.RespondToIssueAsync(id, request.Response, request.NewStatus, cancellationToken);
            return Ok(new { message = "Response sent successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error responding to issue");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Upload photo to issue
    /// </summary>
    [HttpPost("{id}/photos")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AttachmentUploadResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> UploadPhoto(
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
            var storagePath = await _issueService.UploadPhotoAsync(
                id, stream, file.FileName, file.ContentType, file.Length, cancellationToken);

            return Ok(new { fileName = file.FileName, storagePath, size = file.Length });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading photo");
            return BadRequest(new { message = ex.Message });
        }
    }
}

public class RespondToIssueRequest
{
    public string Response { get; set; } = string.Empty;
    public string NewStatus { get; set; } = "Resolved";
}
