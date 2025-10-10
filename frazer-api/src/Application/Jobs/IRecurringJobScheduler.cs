namespace FrazerDealer.Application.Jobs;

public interface IRecurringJobScheduler
{
    Task ScheduleDefaultsAsync(CancellationToken cancellationToken);
}
