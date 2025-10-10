namespace FrazerDealer.Contracts.Auth;

public record LoginResponse(string AccessToken, DateTime ExpiresAt, string RefreshToken, IEnumerable<string> Roles);
