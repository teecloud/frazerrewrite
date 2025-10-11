using System.Linq;
using FrazerDealer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FrazerDealer.Infrastructure.Persistence.Seed;

public static class SeedData
{
    public static async Task SeedAsync(AppDbContext context, ILogger logger, CancellationToken cancellationToken)
    {
        await context.Database.MigrateAsync(cancellationToken);

        if (!await context.Vehicles.AnyAsync(cancellationToken))
        {
            var vehicles = new List<Vehicle>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    StockNumber = "FZ-1001",
                    Vin = "1FATP8UH3K5123456",
                    Year = "2023",
                    Make = "Ford",
                    Model = "Mustang",
                    Trim = "Premium",
                    Price = 41500m,
                    Cost = 36500m,
                    DateArrived = DateTime.UtcNow.AddDays(-10)
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    StockNumber = "FZ-1002",
                    Vin = "1C4RJFBG8LC123456",
                    Year = "2022",
                    Make = "Jeep",
                    Model = "Grand Cherokee",
                    Trim = "Limited",
                    Price = 50500m,
                    Cost = 44200m,
                    DateArrived = DateTime.UtcNow.AddDays(-4)
                }
            };

            await context.Vehicles.AddRangeAsync(vehicles, cancellationToken);
        }

        if (!await context.Customers.AnyAsync(cancellationToken))
        {
            var customers = new List<Customer>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Jamie",
                    LastName = "Wheeler",
                    Email = "jamie.wheeler@example.com",
                    Phone = "555-0100",
                    Address = "123 Main St",
                    City = "Frazer",
                    State = "TX",
                    PostalCode = "75035"
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Morgan",
                    LastName = "Hughes",
                    Email = "morgan.hughes@example.com",
                    Phone = "555-0200",
                    Address = "987 Elm St",
                    City = "Plano",
                    State = "TX",
                    PostalCode = "75024"
                }
            };

            await context.Customers.AddRangeAsync(customers, cancellationToken);
        }

        if (!await context.InsuranceProviders.AnyAsync(cancellationToken))
        {
            await context.InsuranceProviders.AddRangeAsync(new[]
            {
                new InsuranceProvider
                {
                    Id = Guid.NewGuid(),
                    Name = "Frazer Insurance Group",
                    Email = "support@frazerinsure.test",
                    Phone = "555-0300",
                    Notes = "Primary carrier for bundled policies"
                },
                new InsuranceProvider
                {
                    Id = Guid.NewGuid(),
                    Name = "Sunset Mutual",
                    Email = "dealer@sunsetmutual.test",
                    Phone = "555-0315",
                    Notes = "Preferred gap coverage"
                }
            }, cancellationToken);
        }

        if (!await context.Prospects.AnyAsync(cancellationToken))
        {
            var availableVehicles = context.Vehicles.Local.ToList();
            if (availableVehicles.Count == 0)
            {
                availableVehicles = await context.Vehicles.ToListAsync(cancellationToken);
            }

            var prospects = new List<Prospect>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Alex Johnson",
                    Email = "alex.johnson@example.com",
                    Phone = "555-0400",
                    Vehicles = availableVehicles.Take(1).ToList()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Riley Chen",
                    Email = "riley.chen@example.com",
                    Phone = "555-0500",
                    Vehicles = availableVehicles.Skip(1).Take(1).ToList()
                }
            };

            await context.Prospects.AddRangeAsync(prospects, cancellationToken);
        }

        await context.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Database seeded successfully.");
    }
}
