using FrazerDealer.Application.Interfaces;
using FrazerDealer.Contracts.Inventory;
using FrazerDealer.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrazerDealer.Api.Controllers;

[ApiController]
[Route("api/vehicles")]
[Authorize]
public class VehiclesController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

    public VehiclesController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<VehicleSummary>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetVehicles(CancellationToken cancellationToken)
    {
        var result = await _vehicleService.GetInventoryAsync(cancellationToken);
        if (!result.Succeeded || result.Value is null)
        {
            return Problem(detail: string.Join(';', result.Errors));
        }

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(VehicleDetail), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetVehicle(Guid id, CancellationToken cancellationToken)
    {
        var result = await _vehicleService.GetVehicleAsync(id, cancellationToken);
        if (!result.Succeeded || result.Value is null)
        {
            return NotFound(result.Errors);
        }

        return Ok(result.Value);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Admin + "," + Roles.Manager)]
    [ProducesResponseType(typeof(VehicleDetail), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleRequest request, CancellationToken cancellationToken)
    {
        var result = await _vehicleService.CreateVehicleAsync(request, cancellationToken);
        if (!result.Succeeded || result.Value is null)
        {
            return BadRequest(result.Errors);
        }

        return CreatedAtAction(nameof(GetVehicle), new { id = result.Value.Id }, result.Value);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = Roles.Admin + "," + Roles.Manager)]
    public async Task<IActionResult> UpdateVehicle(Guid id, [FromBody] UpdateVehicleRequest request, CancellationToken cancellationToken)
    {
        var result = await _vehicleService.UpdateVehicleAsync(id, request, cancellationToken);
        if (!result.Succeeded)
        {
            return NotFound(result.Errors);
        }

        return NoContent();
    }

    [HttpPost("{id:guid}:mark-sold")]
    [Authorize(Roles = Roles.Manager + "," + Roles.Sales)]
    public async Task<IActionResult> MarkVehicleSold(Guid id, CancellationToken cancellationToken)
    {
        var result = await _vehicleService.MarkVehicleSoldAsync(id, cancellationToken);
        if (!result.Succeeded)
        {
            return NotFound(result.Errors);
        }

        return NoContent();
    }
}
