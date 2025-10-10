namespace FrazerDealer.Application.Interfaces.Adapters;

public interface ITextMaxxSms
{
    Task SendMessageAsync(string phoneNumber, string message, CancellationToken cancellationToken);
}
