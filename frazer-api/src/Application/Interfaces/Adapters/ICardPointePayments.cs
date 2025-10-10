namespace FrazerDealer.Application.Interfaces.Adapters;

public interface ICardPointePayments
{
    Task<string> AuthorizeAsync(decimal amount, CancellationToken cancellationToken);
    Task<bool> CaptureAsync(string authorizationCode, CancellationToken cancellationToken);
}
