using FrazerDealer.Application.Interfaces;
using FrazerDealer.Contracts.Reports;
using FrazerDealer.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrazerDealer.Api.Controllers;

[ApiController]
[Route("api/reports")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IReportsService _reportsService;

    public ReportsController(IReportsService reportsService)
    {
        _reportsService = reportsService;
    }

    [HttpGet("inventory")]
    [ProducesResponseType(typeof(IEnumerable<InventoryReportRow>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInventory(CancellationToken cancellationToken)
    {
        var result = await _reportsService.GetInventoryReportAsync(cancellationToken);
        if (!result.Succeeded || result.Value is null)
        {
            return Problem(detail: string.Join(';', result.Errors));
        }

        return Ok(result.Value);
    }

    [HttpGet("titles-pending")]
    [ProducesResponseType(typeof(IEnumerable<TitlesPendingReportRow>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTitlesPending([FromQuery] string? filter, CancellationToken cancellationToken)
    {
        var result = await _reportsService.GetTitlesPendingReportAsync(filter, cancellationToken);
        if (!result.Succeeded || result.Value is null)
        {
            return Problem(detail: string.Join(';', result.Errors));
        }

        return Ok(result.Value);
    }

    [HttpGet("insurance")]
    [ProducesResponseType(typeof(IEnumerable<InsuranceReportRow>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInsurance(CancellationToken cancellationToken)
    {
        var result = await _reportsService.GetInsuranceReportAsync(cancellationToken);
        if (!result.Succeeded || result.Value is null)
        {
            return Problem(detail: string.Join(';', result.Errors));
        }

        return Ok(result.Value);
    }
}
