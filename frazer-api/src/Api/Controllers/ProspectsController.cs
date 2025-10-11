using FrazerDealer.Application.Interfaces;
using FrazerDealer.Contracts.Prospects;
using FrazerDealer.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrazerDealer.Api.Controllers;

[ApiController]
[Route("api/prospects")]
[Authorize]
public class ProspectsController : ControllerBase
{
    private readonly IProspectService _prospectService;

    public ProspectsController(IProspectService prospectService)
    {
        _prospectService = prospectService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProspectSummary>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProspects(CancellationToken cancellationToken)
    {
        var result = await _prospectService.GetProspectsAsync(cancellationToken);
        if (!result.Succeeded || result.Value is null)
        {
            return Problem(detail: string.Join(';', result.Errors));
        }

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProspectSummary), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProspect(Guid id, CancellationToken cancellationToken)
    {
        var result = await _prospectService.GetProspectAsync(id, cancellationToken);
        if (!result.Succeeded || result.Value is null)
        {
            return NotFound(result.Errors);
        }

        return Ok(result.Value);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Admin + "," + Roles.Manager + "," + Roles.Sales)]
    [ProducesResponseType(typeof(ProspectSummary), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateProspect([FromBody] CreateProspectRequest request, CancellationToken cancellationToken)
    {
        var result = await _prospectService.CreateProspectAsync(request, cancellationToken);
        if (!result.Succeeded || result.Value is null)
        {
            return BadRequest(result.Errors);
        }

        return CreatedAtAction(nameof(GetProspect), new { id = result.Value.Id }, result.Value);
    }
}
