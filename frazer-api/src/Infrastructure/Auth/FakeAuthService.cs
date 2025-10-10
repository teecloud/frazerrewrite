using System.Security.Cryptography;
using FrazerDealer.Application.Common;
using FrazerDealer.Application.Interfaces;
using FrazerDealer.Contracts.Auth;
using Microsoft.Extensions.Options;

namespace FrazerDealer.Infrastructure.Auth;

public class FakeAuthService : IAuthService
{
    private readonly JwtOptions _options;

    public FakeAuthService(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return Task.FromResult(Result<LoginResponse>.Failure("Invalid credentials"));
        }

        var roles = request.Username.ToLowerInvariant() switch
        {
            "admin" => new[] { Roles.Admin, Roles.Manager },
            "manager" => new[] { Roles.Manager, Roles.Sales },
            "sales" => new[] { Roles.Sales },
            "clerk" => new[] { Roles.Clerk },
            "service" => new[] { Roles.Service },
            _ => new[] { Roles.Sales }
        };

        var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        var response = new LoginResponse(token, DateTime.UtcNow.AddHours(1), Guid.NewGuid().ToString("N"), roles);
        return Task.FromResult(Result<LoginResponse>.Success(response));
    }
}

public static class Roles
{
    public const string Admin = "Admin";
    public const string Manager = "Manager";
    public const string Sales = "Sales";
    public const string Clerk = "Clerk";
    public const string Service = "Service";
}

public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = "frazer";
    public string Audience { get; set; } = "frazer-clients";
    public string SigningKey { get; set; } = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
}
