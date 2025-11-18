using Microsoft.AspNetCore.Mvc;
using PugPlatform.Application.DTOs.GeoJson;
using PugPlatform.Application.DTOs.Parcels;
using PugPlatform.Application.Interfaces;

namespace PugPlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParcelsController : ControllerBase
{
    private readonly IParcelService _parcelService;
    private readonly ILogger<ParcelsController> _logger;

    public ParcelsController(IParcelService parcelService, ILogger<ParcelsController> logger)
    {
        _parcelService = parcelService;
        _logger = logger;
    }

    /// <summary>
    /// Get parcel by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ParcelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var parcel = await _parcelService.GetParcelByIdAsync(id, cancellationToken);
        if (parcel == null)
        {
            return NotFound(new { message = "Parcel not found" });
        }

        return Ok(parcel);
    }

    /// <summary>
    /// Get parcel by cadastral ID
    /// </summary>
    [HttpGet("cadastral/{parcelId}")]
    [ProducesResponseType(typeof(ParcelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByCadastralId(string parcelId, CancellationToken cancellationToken)
    {
        var parcel = await _parcelService.GetParcelByCadastralIdAsync(parcelId, cancellationToken);
        if (parcel == null)
        {
            return NotFound(new { message = "Parcel not found" });
        }

        return Ok(parcel);
    }

    /// <summary>
    /// Get parcels by bounding box
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(GeoJsonFeatureCollection), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByBbox(
        [FromQuery] double minLng,
        [FromQuery] double minLat,
        [FromQuery] double maxLng,
        [FromQuery] double maxLat,
        [FromQuery] string? q = null,
        CancellationToken cancellationToken = default)
    {
        var parcels = await _parcelService.GetParcelsByBboxAsync(
            minLng, minLat, maxLng, maxLat, q, cancellationToken);

        return Ok(parcels);
    }
}
