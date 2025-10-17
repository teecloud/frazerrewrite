using System.Linq;
using FrazerDealer.Domain.Entities;
using FrazerDealer.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FrazerDealer.Infrastructure.Persistence.Seed;

public static class SeedData
{
    public static async Task SeedAsync(AppDbContext context, ILogger logger, CancellationToken cancellationToken)
    {
        await context.Database.MigrateAsync(cancellationToken);

        await EnsureSalesSchemaAsync(context, logger, cancellationToken);

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
                    DateArrived = DateTime.UtcNow.AddDays(-10),
                    Photos =
                    {
                        new Photo
                        {
                            Id = Guid.NewGuid(),
                            Url = "https://images.frazer.test/inventory/fz-1001-primary.jpg",
                            Caption = "Front three-quarter view",
                            IsPrimary = true
                        },
                        new Photo
                        {
                            Id = Guid.NewGuid(),
                            Url = "https://images.frazer.test/inventory/fz-1001-interior.jpg",
                            Caption = "Premium interior",
                            IsPrimary = false
                        }
                    }
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
                    DateArrived = DateTime.UtcNow.AddDays(-4),
                    Photos =
                    {
                        new Photo
                        {
                            Id = Guid.NewGuid(),
                            Url = "https://images.frazer.test/inventory/fz-1002-primary.jpg",
                            Caption = "Dealer lot exterior",
                            IsPrimary = true
                        }
                    }
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

    private static async Task EnsureSalesSchemaAsync(AppDbContext context, ILogger logger, CancellationToken cancellationToken)
    {
        const string ensureSalesSql = """
IF OBJECT_ID(N'[dbo].[Sales]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Sales]
    (
        [Id] UNIQUEIDENTIFIER NOT NULL,
        [VehicleId] UNIQUEIDENTIFIER NOT NULL,
        [CustomerId] UNIQUEIDENTIFIER NOT NULL,
        [Status] INT NOT NULL,
        [CreatedOn] DATETIME2 NOT NULL,
        [CompletedOn] DATETIME2 NULL,
        [Subtotal] DECIMAL(18,2) NOT NULL,
        [FeesTotal] DECIMAL(18,2) NOT NULL,
        [PaymentsTotal] DECIMAL(18,2) NOT NULL,
        CONSTRAINT [PK_Sales] PRIMARY KEY ([Id])
    );

    CREATE INDEX [IX_Sales_CustomerId] ON [dbo].[Sales] ([CustomerId]);
    CREATE INDEX [IX_Sales_VehicleId] ON [dbo].[Sales] ([VehicleId]);

    ALTER TABLE [dbo].[Sales] WITH CHECK
        ADD CONSTRAINT [FK_Sales_Customers_CustomerId]
        FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customers]([Id]) ON DELETE NO ACTION;

    ALTER TABLE [dbo].[Sales] WITH CHECK
        ADD CONSTRAINT [FK_Sales_Vehicles_VehicleId]
        FOREIGN KEY ([VehicleId]) REFERENCES [dbo].[Vehicles]([Id]) ON DELETE NO ACTION;
END;

IF OBJECT_ID(N'[dbo].[Fees]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Fees]
    (
        [Id] UNIQUEIDENTIFIER NOT NULL,
        [SaleId] UNIQUEIDENTIFIER NOT NULL,
        [Code] NVARCHAR(MAX) NOT NULL,
        [Description] NVARCHAR(MAX) NOT NULL,
        [Amount] DECIMAL(18,2) NOT NULL,
        [IsRecurring] BIT NOT NULL,
        CONSTRAINT [PK_Fees] PRIMARY KEY ([Id])
    );

    CREATE INDEX [IX_Fees_SaleId] ON [dbo].[Fees] ([SaleId]);

    ALTER TABLE [dbo].[Fees] WITH CHECK
        ADD CONSTRAINT [FK_Fees_Sales_SaleId]
        FOREIGN KEY ([SaleId]) REFERENCES [dbo].[Sales]([Id]) ON DELETE CASCADE;
END;

IF OBJECT_ID(N'[dbo].[Payments]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Payments]
    (
        [Id] UNIQUEIDENTIFIER NOT NULL,
        [SaleId] UNIQUEIDENTIFIER NOT NULL,
        [Amount] DECIMAL(18,2) NOT NULL,
        [CollectedOn] DATETIME2 NOT NULL,
        [Method] NVARCHAR(MAX) NOT NULL,
        [Status] INT NOT NULL,
        [ExternalReference] NVARCHAR(MAX) NULL,
        CONSTRAINT [PK_Payments] PRIMARY KEY ([Id])
    );

    CREATE INDEX [IX_Payments_SaleId] ON [dbo].[Payments] ([SaleId]);

    ALTER TABLE [dbo].[Payments] WITH CHECK
        ADD CONSTRAINT [FK_Payments_Sales_SaleId]
        FOREIGN KEY ([SaleId]) REFERENCES [dbo].[Sales]([Id]) ON DELETE CASCADE;
END;

IF OBJECT_ID(N'[dbo].[Vehicles]', N'U') IS NOT NULL
    AND COL_LENGTH(N'[dbo].[Vehicles]', 'CurrentSaleId') IS NOT NULL
    AND NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Vehicles_Sales_CurrentSaleId')
BEGIN
    ALTER TABLE [dbo].[Vehicles] WITH CHECK
        ADD CONSTRAINT [FK_Vehicles_Sales_CurrentSaleId]
        FOREIGN KEY ([CurrentSaleId]) REFERENCES [dbo].[Sales]([Id]) ON DELETE SET NULL;

    ALTER TABLE [dbo].[Vehicles] CHECK CONSTRAINT [FK_Vehicles_Sales_CurrentSaleId];
END;
""";

        try
        {
            await context.Database.ExecuteSqlRawAsync(ensureSalesSql, cancellationToken);
        }
        catch (SqlException ex)
        {
            logger.LogWarning(ex, "Failed to ensure sales schema exists. {Message}", ex.Message);
        }
    }
}
