using FrazerDealer.Application.Interfaces;
using FrazerDealer.Contracts.Customers;
using FrazerDealer.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrazerDealer.Api.Controllers;

[ApiController]
[Route("api/customers")]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CustomerSummary>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCustomers(CancellationToken cancellationToken)
    {
        var result = await _customerService.GetCustomersAsync(cancellationToken);
        if (!result.Succeeded || result.Value is null)
        {
            return Problem(detail: string.Join(';', result.Errors));
        }

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CustomerDetail), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCustomer(Guid id, CancellationToken cancellationToken)
    {
        var result = await _customerService.GetCustomerAsync(id, cancellationToken);
        if (!result.Succeeded || result.Value is null)
        {
            return NotFound(result.Errors);
        }

        return Ok(result.Value);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Admin + "," + Roles.Manager + "," + Roles.Sales)]
    [ProducesResponseType(typeof(CustomerDetail), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var result = await _customerService.CreateCustomerAsync(request, cancellationToken);
        if (!result.Succeeded || result.Value is null)
        {
            return BadRequest(result.Errors);
        }

        return CreatedAtAction(nameof(GetCustomer), new { id = result.Value.Id }, result.Value);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = Roles.Admin + "," + Roles.Manager + "," + Roles.Clerk)]
    public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        var result = await _customerService.UpdateCustomerAsync(id, request, cancellationToken);
        if (!result.Succeeded)
        {
            return NotFound(result.Errors);
        }

        return NoContent();
    }
}
