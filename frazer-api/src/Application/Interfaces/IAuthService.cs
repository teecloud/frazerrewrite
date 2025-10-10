using FrazerDealer.Application.Common;
using FrazerDealer.Contracts.Auth;

namespace FrazerDealer.Application.Interfaces;

public interface IAuthService
{
    Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
}
