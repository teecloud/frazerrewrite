namespace FrazerDealer.Application.Interfaces.Adapters;

public interface IFrazerHubClient
{
    Task PushInventoryAsync(CancellationToken cancellationToken);
}
