namespace FrazerDealer.Application.Interfaces.Adapters;

public interface ISiriusXmApi
{
    Task RegisterTrialAsync(string vin, CancellationToken cancellationToken);
}
