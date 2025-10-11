using System.Net;
using FrazerDealer.Application.Interfaces;
using FrazerDealer.Application.Interfaces.Adapters;
using FrazerDealer.Application.Jobs;
using FrazerDealer.Infrastructure.Adapters;
using FrazerDealer.Infrastructure.Auth;
using FrazerDealer.Infrastructure.Jobs;
using FrazerDealer.Infrastructure.Persistence;
using FrazerDealer.Infrastructure.Persistence.Seed;
using FrazerDealer.Infrastructure.Services;
using Hangfire.MemoryStorage;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace FrazerDealer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Connection string 'Default' not found.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();

        services.AddHangfire((serviceProvider, config) =>
        {
            var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("HangfireConfiguration");
            var hangfireConnectionString = configuration.GetConnectionString("Hangfire") ?? connectionString;

            config.UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings();

            if (IsResolvableHangfireConnection(hangfireConnectionString, logger, out var reason))
            {
                config.UsePostgreSqlStorage(hangfireConnectionString);
            }
            else
            {
                logger.LogWarning("{Reason} Falling back to in-memory Hangfire storage.", reason);
                config.UseMemoryStorage();
            }
        });

        services.AddHangfireServer();

        services.AddScoped<IAuthService, FakeAuthService>();
        services.AddScoped<IVehicleService, FakeVehicleService>();
        services.AddScoped<ICustomerService, FakeCustomerService>();
        services.AddScoped<ISaleService, FakeSaleService>();
        services.AddScoped<IFeeService, FakeFeeService>();
        services.AddScoped<IInsuranceService, FakeInsuranceService>();
        services.AddScoped<IPaymentService, FakePaymentService>();
        services.AddScoped<IReportsService, FakeReportsService>();
        services.AddScoped<IJobService, FakeJobService>();
        services.AddScoped<IRecurringJobScheduler, HangfireRecurringJobScheduler>();
        services.AddSingleton<IFrazerHubClient, FakeFrazerHubClient>();
        services.AddSingleton<ITextMaxxSms, FakeTextMaxxSms>();
        services.AddSingleton<ISiriusXmApi, FakeSiriusXmApi>();
        services.AddSingleton<ICardPointePayments, FakeCardPointePayments>();

        services.AddOptions<AdapterOptions>().Bind(configuration.GetSection(AdapterOptions.SectionName));

        services.AddHealthChecks().AddDbContextCheck<AppDbContext>();

        return services;
    }

    public static async Task InitialiseDatabaseAsync(this IServiceProvider provider, CancellationToken cancellationToken = default)
    {
        using var scope = provider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DatabaseInitializer");
        await SeedData.SeedAsync(context, logger, cancellationToken);
    }

    private static bool IsResolvableHangfireConnection(string connectionString, ILogger logger, out string reason)
    {
        try
        {
            var builder = new NpgsqlConnectionStringBuilder(connectionString);

            if (string.IsNullOrWhiteSpace(builder.Host))
            {
                reason = "Hangfire connection string is missing a host.";
                return false;
            }

            if (IPAddress.TryParse(builder.Host, out _))
            {
                reason = string.Empty;
                return true;
            }

            try
            {
                _ = Dns.GetHostEntry(builder.Host);
                reason = string.Empty;
                return true;
            }
            catch (SocketException ex)
            {
                logger.LogDebug(ex, "Failed to resolve Hangfire host '{Host}'.", builder.Host);
                reason = $"Unable to resolve Hangfire host '{builder.Host}'.";
                return false;
            }
        }
        catch (Exception ex) when (ex is ArgumentException or FormatException)
        {
            logger.LogDebug(ex, "Invalid Hangfire connection string provided.");
            reason = "Invalid Hangfire connection string provided.";
            return false;
        }
    }
}

public interface IDatabaseInitializer
{
    Task InitialiseAsync(CancellationToken cancellationToken);
}

internal class DatabaseInitializer : IDatabaseInitializer
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task InitialiseAsync(CancellationToken cancellationToken)
    {
        await _serviceProvider.InitialiseDatabaseAsync(cancellationToken);
    }
}
