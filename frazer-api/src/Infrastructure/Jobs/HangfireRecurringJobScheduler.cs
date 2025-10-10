using FrazerDealer.Application.Jobs;
using Hangfire;
using Microsoft.Extensions.Logging;

namespace FrazerDealer.Infrastructure.Jobs;

public class HangfireRecurringJobScheduler : IRecurringJobScheduler
{
    private readonly ILogger<HangfireRecurringJobScheduler> _logger;

    public HangfireRecurringJobScheduler(ILogger<HangfireRecurringJobScheduler> logger)
    {
        _logger = logger;
    }

    public Task ScheduleDefaultsAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Configuring default recurring jobs");

        RecurringJob.AddOrUpdate("ApplyRecurringFees", () => Console.WriteLine("ApplyRecurringFees executed"), "0 6 * * *");
        RecurringJob.AddOrUpdate("AutoUploads", () => Console.WriteLine("AutoUploads executed"), "0 * * * *");
        RecurringJob.AddOrUpdate("BackupTrigger", () => Console.WriteLine("BackupTrigger executed"), "0 2 * * *");
        RecurringJob.AddOrUpdate("PaymentSettlementPoll", () => Console.WriteLine("PaymentSettlementPoll executed"), "*/15 * * * *");

        return Task.CompletedTask;
    }
}
