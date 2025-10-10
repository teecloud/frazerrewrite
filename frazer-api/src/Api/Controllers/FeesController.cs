using FrazerDealer.Application.Interfaces;
using FrazerDealer.Contracts.Fees;
using FrazerDealer.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrazerDealer.Api.Controllers;

[ApiController]
[Route("api/fees")]
[Authorize(Roles = Roles.Admin + "," + Roles.Manager)]
public class FeesController : ControllerBase
{
    private readonly IFeeService _feeService;

    public FeesController(IFeeService feeService)
    {
        _feeService = feeService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FeeConfigurationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFees(CancellationToken cancellationToken)
    {
        var result = await _feeService.GetFeesAsync(cancellationToken);
        if (!result.Succeeded || result.Value is null)
        {
            return Problem(detail: string.Join(';', result.Errors));
        }

        return Ok(result.Value);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateFee(Guid id, [FromBody] UpdateFeeConfigurationRequest request, CancellationToken cancellationToken)
    {
        var result = await _feeService.UpdateFeeAsync(id, request, cancellationToken);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return NoContent();
    }
}
