using System.Data.Common;
using System.Net;
using System.Net.Sockets;
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
using Hangfire.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;

namespace FrazerDealer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Connection string 'Default' not found.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();

        services.AddHangfire((serviceProvider, config) =>
        {
            var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("HangfireConfiguration");
            var hangfireConnectionString = configuration.GetConnectionString("Hangfire") ?? connectionString;

            config.UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings();

            if (IsResolvableHangfireConnection(hangfireConnectionString, logger, out var reason))
            {
                config.UseSqlServerStorage(hangfireConnectionString);
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
        services.AddScoped<IProspectService, FakeProspectService>();
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
        try
        {
            if (!await context.Database.CanConnectAsync(cancellationToken))
            {
                logger.LogWarning("Skipping database initialisation because the SQL Server instance is unavailable. Verify the connection string and server accessibility.");
                return;
            }

            await SeedData.SeedAsync(context, logger, cancellationToken);
        }
        catch (SqlException ex)
        {
            logger.LogError(ex, "Failed to initialise the database. Ensure the configured SQL Server is available and the connection string is correct.");
        }
        catch (DbException ex)
        {
            logger.LogError(ex, "Failed to initialise the database. Ensure the configured SQL Server is available and the connection string is correct.");
        }
        catch (TimeoutException ex)
        {
            logger.LogError(ex, "Failed to initialise the database. Ensure the configured SQL Server is available and the connection string is correct.");
        }
        catch (InvalidOperationException ex)
        {
            logger.LogError(ex, "Failed to initialise the database. Ensure the configured SQL Server is available and the connection string is correct.");
        }
    }

    private static bool IsResolvableHangfireConnection(string connectionString, ILogger logger, out string reason)
    {
        try
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            var dataSource = builder.DataSource?.Trim();

            if (string.IsNullOrWhiteSpace(dataSource))
            {
                reason = "Hangfire connection string is missing a server.";
                return false;
            }

            var host = dataSource;

            if (host.StartsWith("tcp:", StringComparison.OrdinalIgnoreCase))
            {
                host = host[4..];
            }

            var instanceSeparatorIndex = host.IndexOf('\\');
            if (instanceSeparatorIndex >= 0)
            {
                host = host[..instanceSeparatorIndex];
            }

            var portSeparatorIndex = host.IndexOf(',');
            if (portSeparatorIndex >= 0)
            {
                host = host[..portSeparatorIndex];
            }

            host = host.Trim();

            if (string.IsNullOrWhiteSpace(host))
            {
                reason = "Hangfire connection string is missing a server host.";
                return false;
            }

            var hostResolvable = false;

            if (host is "." or "(local)" || host.Equals("localhost", StringComparison.OrdinalIgnoreCase) ||
                host.StartsWith("(localdb)", StringComparison.OrdinalIgnoreCase))
            {
                hostResolvable = true;
            }
            else if (IPAddress.TryParse(host, out _))
            {
                hostResolvable = true;
            }
            else
            {
                try
                {
                    _ = Dns.GetHostEntry(host);
                    hostResolvable = true;
                }
                catch (SocketException ex)
                {
                    logger.LogDebug(ex, "Failed to resolve Hangfire host '{Host}'.", host);
                    reason = $"Unable to resolve Hangfire host '{host}'.";
                    return false;
                }
            }

            if (!hostResolvable)
            {
                reason = $"Unable to resolve Hangfire host '{host}'.";
                return false;
            }

            if (!CanEstablishHangfireConnection(builder, logger, out reason))
            {
                return false;
            }

            reason = string.Empty;
            return true;
        }
        catch (Exception ex) when (ex is ArgumentException or FormatException)
        {
            logger.LogDebug(ex, "Invalid Hangfire connection string provided.");
            reason = "Invalid Hangfire connection string provided.";
            return false;
        }
    }

    private static bool CanEstablishHangfireConnection(SqlConnectionStringBuilder builder, ILogger logger, out string reason)
    {
        try
        {
            var testBuilder = new SqlConnectionStringBuilder(builder.ConnectionString);
            var configuredTimeout = testBuilder.ConnectTimeout;

            if (configuredTimeout <= 0 || configuredTimeout > 5)
            {
                testBuilder.ConnectTimeout = 5;
            }

            using var connection = new SqlConnection(testBuilder.ConnectionString);
            connection.Open();

            reason = string.Empty;
            return true;
        }
        catch (Exception ex) when (ex is SqlException or InvalidOperationException or TimeoutException)
        {
            logger.LogDebug(ex, "Unable to connect to Hangfire SQL Server using the provided connection string.");
            reason = $"Unable to connect to Hangfire SQL Server. {ex.Message}";
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
