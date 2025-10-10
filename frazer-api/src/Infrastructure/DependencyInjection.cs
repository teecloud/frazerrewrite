using FrazerDealer.Application.Interfaces;
using FrazerDealer.Application.Interfaces.Adapters;
using FrazerDealer.Application.Jobs;
using FrazerDealer.Infrastructure.Adapters;
using FrazerDealer.Infrastructure.Auth;
using FrazerDealer.Infrastructure.Services;
using FrazerDealer.Infrastructure.Jobs;
using FrazerDealer.Infrastructure.Persistence;
using FrazerDealer.Infrastructure.Persistence.Seed;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

        services.AddHangfire(config =>
        {
            config.UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(connectionString);
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
