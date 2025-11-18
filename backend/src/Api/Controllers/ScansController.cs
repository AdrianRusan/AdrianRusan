using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PugPlatform.Application.DTOs.Scans;
using PugPlatform.Application.Interfaces;
using System.Security.Claims;

namespace PugPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScansController : ControllerBase
{
    private readonly I3DScanService _scanService;
    private readonly ILogger<ScansController> _logger;

    public ScansController(I3DScanService scanService, ILogger<ScansController> logger)
    {
        _scanService = scanService;
        _logger = logger;
    }

    /// <summary>
    /// Create a new 3D scan request
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ScanRequestDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateScanRequest(
        [FromBody] CreateScanRequestDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = GetUserId();
            var scanRequest = await _scanService.CreateScanRequestAsync(request, userId, cancellationToken);

            return CreatedAtAction(nameof(GetScanRequest), new { id = scanRequest.Id }, scanRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating scan request");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get scan request by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ScanRequestDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetScanRequest(Guid id, CancellationToken cancellationToken)
    {
        var scanRequest = await _scanService.GetScanRequestByIdAsync(id, cancellationToken);
        if (scanRequest == null)
        {
            return NotFound(new { message = "Scan request not found" });
        }

        return Ok(scanRequest);
    }

    /// <summary>
    /// Get scans by parcel ID
    /// </summary>
    [HttpGet("parcel/{parcelId}")]
    [ProducesResponseType(typeof(List<ScanFileDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetScansByParcel(Guid parcelId, CancellationToken cancellationToken)
    {
        var scans = await _scanService.GetScansByParcelIdAsync(parcelId, cancellationToken);
        return Ok(scans);
    }

    /// <summary>
    /// Upload scan file (admin only)
    /// </summary>
    [HttpPost("{id}/files")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    [RequestSizeLimit(500 * 1024 * 1024)] // 500MB limit
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> UploadScanFile(
        Guid id,
        [FromForm] IFormFile file,
        CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { message = "No file uploaded" });
        }

        // Validate file type
        var allowedExtensions = new[] { ".las", ".laz", ".ply", ".obj" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(extension))
        {
            return BadRequest(new { message = $"File type not allowed. Allowed types: {string.Join(", ", allowedExtensions)}" });
        }

        try
        {
            using var stream = file.OpenReadStream();
            var storagePath = await _scanService.UploadScanFileAsync(
                id, stream, file.FileName, file.ContentType, file.Length, cancellationToken);

            return Ok(new { fileName = file.FileName, storagePath, size = file.Length });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading scan file");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Download scan file
    /// </summary>
    [HttpGet("{scanId}/files/{fileId}/download")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> DownloadScanFile(
        Guid scanId,
        Guid fileId,
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = User.Identity?.IsAuthenticated == true ? GetUserId() : (Guid?)null;
            var downloadUrl = await _scanService.GetScanFileDownloadUrlAsync(scanId, fileId, userId, cancellationToken);

            return Ok(new { downloadUrl });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting download URL");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Mark scan request as completed (admin only)
    /// </summary>
    [HttpPost("{id}/complete")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CompleteScanRequest(
        Guid id,
        [FromBody] CompleteScanRequestDto? request,
        CancellationToken cancellationToken)
    {
        try
        {
            await _scanService.CompleteScanRequestAsync(
                id, request?.NotifyUser ?? true, cancellationToken);

            return Ok(new { message = "Scan request marked as completed" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error completing scan request");
            return BadRequest(new { message = ex.Message });
        }
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim ?? throw new UnauthorizedAccessException("User ID not found"));
    }
}

public class CompleteScanRequestDto
{
    public bool NotifyUser { get; set; } = true;
}
