using FrazerDealer.Application.Interfaces;
using FrazerDealer.Contracts.Photos;
using FrazerDealer.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrazerDealer.Api.Controllers;

[ApiController]
[Route("api/photos")]
[Authorize]
public class PhotosController : ControllerBase
{
    private readonly IPhotoService _photoService;

    public PhotosController(IPhotoService photoService)
    {
        _photoService = photoService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PhotoSummary>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPhotos([FromQuery] Guid? vehicleId, CancellationToken cancellationToken)
    {
        var result = await _photoService.GetPhotosAsync(vehicleId, cancellationToken);
        if (!result.Succeeded || result.Value is null)
        {
            return Problem(detail: string.Join(';', result.Errors));
        }

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PhotoDetail), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPhoto(Guid id, CancellationToken cancellationToken)
    {
        var result = await _photoService.GetPhotoAsync(id, cancellationToken);
        if (!result.Succeeded || result.Value is null)
        {
            return NotFound(result.Errors);
        }

        return Ok(result.Value);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Admin + "," + Roles.Manager)]
    [ProducesResponseType(typeof(PhotoDetail), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreatePhoto([FromBody] CreatePhotoRequest request, CancellationToken cancellationToken)
    {
        var result = await _photoService.CreatePhotoAsync(request, cancellationToken);
        if (!result.Succeeded || result.Value is null)
        {
            return BadRequest(result.Errors);
        }

        return CreatedAtAction(nameof(GetPhoto), new { id = result.Value.Id }, result.Value);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = Roles.Admin + "," + Roles.Manager)]
    public async Task<IActionResult> UpdatePhoto(Guid id, [FromBody] UpdatePhotoRequest request, CancellationToken cancellationToken)
    {
        var result = await _photoService.UpdatePhotoAsync(id, request, cancellationToken);
        if (!result.Succeeded)
        {
            return NotFound(result.Errors);
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = Roles.Admin + "," + Roles.Manager)]
    public async Task<IActionResult> DeletePhoto(Guid id, CancellationToken cancellationToken)
    {
        var result = await _photoService.DeletePhotoAsync(id, cancellationToken);
        if (!result.Succeeded)
        {
            return NotFound(result.Errors);
        }

        return NoContent();
    }
}
