using FrazerDealer.Application.Interfaces;
using FrazerDealer.Contracts.Jobs;
using FrazerDealer.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrazerDealer.Api.Controllers;

[ApiController]
[Route("api/jobs")]
[Authorize(Roles = Roles.Admin + "," + Roles.Manager)]
public class JobsController : ControllerBase
{
    private readonly IJobService _jobService;

    public JobsController(IJobService jobService)
    {
        _jobService = jobService;
    }

    [HttpPost("enqueue")]
    [ProducesResponseType(typeof(JobStatusDto), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Enqueue([FromBody] JobEnqueueRequest request, CancellationToken cancellationToken)
    {
        var result = await _jobService.EnqueueJobAsync(request, cancellationToken);
        if (!result.Succeeded || result.Value is null)
        {
            return BadRequest(result.Errors);
        }

        return Accepted(result.Value);
    }

    [HttpGet("status/{id:guid}")]
    [ProducesResponseType(typeof(JobStatusDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStatus(Guid id, CancellationToken cancellationToken)
    {
        var result = await _jobService.GetStatusAsync(id, cancellationToken);
        if (!result.Succeeded || result.Value is null)
        {
            return NotFound(result.Errors);
        }

        return Ok(result.Value);
    }

    [HttpGet("history")]
    [ProducesResponseType(typeof(IEnumerable<JobStatusDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHistory([FromQuery] string? type, CancellationToken cancellationToken)
    {
        var result = await _jobService.GetHistoryAsync(new JobHistoryRequest(type), cancellationToken);
        if (!result.Succeeded || result.Value is null)
        {
            return Problem(detail: string.Join(';', result.Errors));
        }

        return Ok(result.Value);
    }
}
