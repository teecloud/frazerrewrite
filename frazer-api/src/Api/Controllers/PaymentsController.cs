using FrazerDealer.Application.Interfaces;
using FrazerDealer.Contracts.Payments;
using FrazerDealer.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrazerDealer.Api.Controllers;

[ApiController]
[Route("api/payments")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PaymentDashboardItem>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDashboard(CancellationToken cancellationToken)
    {
        var result = await _paymentService.GetDashboardAsync(cancellationToken);
        if (!result.Succeeded || result.Value is null)
        {
            return Problem(detail: string.Join(';', result.Errors));
        }

        return Ok(result.Value);
    }

    [HttpPost("{id:guid}:retry")]
    [Authorize(Roles = Roles.Manager + "," + Roles.Clerk)]
    public async Task<IActionResult> Retry(Guid id, [FromBody] PaymentRetryRequest request, CancellationToken cancellationToken)
    {
        var result = await _paymentService.RetryPaymentAsync(id, request, cancellationToken);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Accepted();
    }
}
