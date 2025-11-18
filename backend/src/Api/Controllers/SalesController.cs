using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PugPlatform.Application.DTOs.Sales;
using PugPlatform.Application.Interfaces;
using System.Security.Claims;

namespace PugPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly ISaleService _saleService;
    private readonly ILogger<SalesController> _logger;

    public SalesController(ISaleService saleService, ILogger<SalesController> logger)
    {
        _saleService = saleService;
        _logger = logger;
    }

    /// <summary>
    /// Create a new sale listing
    /// </summary>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(SaleDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateSaleRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = GetUserId();
            var sale = await _saleService.CreateSaleAsync(request, userId, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = sale.Id }, sale);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating sale");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get sale by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SaleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var sale = await _saleService.GetSaleByIdAsync(id, cancellationToken);
        if (sale == null)
        {
            return NotFound(new { message = "Sale not found" });
        }

        return Ok(sale);
    }

    /// <summary>
    /// Get active sales
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<SaleDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] double? minLng,
        [FromQuery] double? minLat,
        [FromQuery] double? maxLng,
        [FromQuery] double? maxLat,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var sales = await _saleService.GetActiveSalesAsync(
            minLng, minLat, maxLng, maxLat, page, pageSize, cancellationToken);

        return Ok(sales);
    }

    /// <summary>
    /// Renew sale listing
    /// </summary>
    [HttpPost("{id}/renew")]
    [Authorize]
    [ProducesResponseType(typeof(SaleDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Renew(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var userId = GetUserId();
            var sale = await _saleService.RenewSaleAsync(id, userId, cancellationToken);

            return Ok(sale);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error renewing sale");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Upload photo to sale
    /// </summary>
    [HttpPost("{id}/photos")]
    [Authorize]
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
            var storagePath = await _saleService.UploadPhotoAsync(
                id, stream, file.FileName, file.ContentType, file.Length, cancellationToken);

            return Ok(new { fileName = file.FileName, storagePath, size = file.Length });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading photo");
            return BadRequest(new { message = ex.Message });
        }
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim ?? throw new UnauthorizedAccessException("User ID not found"));
    }
}
