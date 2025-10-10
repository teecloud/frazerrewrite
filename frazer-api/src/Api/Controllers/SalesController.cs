using FrazerDealer.Application.Interfaces;
using FrazerDealer.Contracts.Sales;
using FrazerDealer.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrazerDealer.Api.Controllers;

[ApiController]
[Route("api/sales")]
[Authorize]
public class SalesController : ControllerBase
{
    private readonly ISaleService _saleService;

    public SalesController(ISaleService saleService)
    {
        _saleService = saleService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SaleSummary>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSales(CancellationToken cancellationToken)
    {
        var result = await _saleService.GetSalesAsync(cancellationToken);
        if (!result.Succeeded || result.Value is null)
        {
            return Problem(detail: string.Join(';', result.Errors));
        }

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SaleDetail), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSale(Guid id, CancellationToken cancellationToken)
    {
        var result = await _saleService.GetSaleAsync(id, cancellationToken);
        if (!result.Succeeded || result.Value is null)
        {
            return NotFound(result.Errors);
        }

        return Ok(result.Value);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Sales + "," + Roles.Manager)]
    [ProducesResponseType(typeof(SaleDetail), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateDraft([FromBody] CreateSaleDraftRequest request, CancellationToken cancellationToken)
    {
        var result = await _saleService.CreateDraftAsync(request, cancellationToken);
        if (!result.Succeeded || result.Value is null)
        {
            return BadRequest(result.Errors);
        }

        return CreatedAtAction(nameof(GetSale), new { id = result.Value.Id }, result.Value);
    }

    [HttpPost("{id:guid}:add-fee")]
    [Authorize(Roles = Roles.Manager + "," + Roles.Sales)]
    public async Task<IActionResult> AddFee(Guid id, [FromBody] AddSaleFeeRequest request, CancellationToken cancellationToken)
    {
        var result = await _saleService.AddFeeAsync(id, request, cancellationToken);
        if (!result.Succeeded || result.Value is null)
        {
            return NotFound(result.Errors);
        }

        return Ok(result.Value);
    }

    [HttpPost("{id:guid}:add-payment")]
    [Authorize(Roles = Roles.Manager + "," + Roles.Sales + "," + Roles.Clerk)]
    public async Task<IActionResult> AddPayment(Guid id, [FromBody] AddSalePaymentRequest request, CancellationToken cancellationToken)
    {
        var result = await _saleService.AddPaymentAsync(id, request, cancellationToken);
        if (!result.Succeeded || result.Value is null)
        {
            return NotFound(result.Errors);
        }

        return Ok(result.Value);
    }

    [HttpPost("{id:guid}:complete")]
    [Authorize(Roles = Roles.Manager + "," + Roles.Admin)]
    public async Task<IActionResult> Complete(Guid id, [FromBody] CompleteSaleRequest request, CancellationToken cancellationToken)
    {
        var result = await _saleService.CompleteSaleAsync(id, request, cancellationToken);
        if (!result.Succeeded || result.Value is null)
        {
            return NotFound(result.Errors);
        }

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}:receipt")]
    [ProducesResponseType(typeof(SaleReceiptDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReceipt(Guid id, CancellationToken cancellationToken)
    {
        var result = await _saleService.GetReceiptAsync(id, cancellationToken);
        if (!result.Succeeded || result.Value is null)
        {
            return NotFound(result.Errors);
        }

        return Ok(result.Value);
    }
}
