using FrazerDealer.Application.Interfaces;
using FrazerDealer.Contracts.Auth;
using Microsoft.AspNetCore.Mvc;

namespace FrazerDealer.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(request, cancellationToken);
        if (!result.Succeeded || result.Value is null)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result.Value);
    }
}
