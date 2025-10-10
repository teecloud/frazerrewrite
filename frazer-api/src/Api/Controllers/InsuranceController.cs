using FrazerDealer.Application.Interfaces;
using FrazerDealer.Contracts.Insurance;
using FrazerDealer.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrazerDealer.Api.Controllers;

[ApiController]
[Route("api/insurance")]
[Authorize]
public class InsuranceController : ControllerBase
{
    private readonly IInsuranceService _insuranceService;

    public InsuranceController(IInsuranceService insuranceService)
    {
        _insuranceService = insuranceService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<InsuranceProviderDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProviders(CancellationToken cancellationToken)
    {
        var result = await _insuranceService.GetProvidersAsync(cancellationToken);
        if (!result.Succeeded || result.Value is null)
        {
            return Problem(detail: string.Join(';', result.Errors));
        }

        return Ok(result.Value);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = Roles.Admin + "," + Roles.Manager)]
    public async Task<IActionResult> UpsertProvider(Guid id, [FromBody] UpsertInsuranceProviderRequest request, CancellationToken cancellationToken)
    {
        var result = await _insuranceService.UpsertProviderAsync(id, request, cancellationToken);
        if (!result.Succeeded || result.Value is null)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result.Value);
    }

    [HttpPut]
    [Authorize(Roles = Roles.Admin + "," + Roles.Manager)]
    public async Task<IActionResult> CreateProvider([FromBody] UpsertInsuranceProviderRequest request, CancellationToken cancellationToken)
    {
        var result = await _insuranceService.UpsertProviderAsync(null, request, cancellationToken);
        if (!result.Succeeded || result.Value is null)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result.Value);
    }
}
