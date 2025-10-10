using Microsoft.Extensions.DependencyInjection;

namespace FrazerDealer.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register MediatR, validators, etc. when implemented.
        return services;
    }
}
