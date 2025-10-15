using FrazerDealer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FrazerDealer.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Prospect> Prospects => Set<Prospect>();
    public DbSet<Sale> Sales => Set<Sale>();
    public DbSet<Fee> Fees => Set<Fee>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<InsuranceProvider> InsuranceProviders => Set<InsuranceProvider>();
    public DbSet<RecurringJob> RecurringJobs => Set<RecurringJob>();
    public DbSet<JobLog> JobLogs => Set<JobLog>();
    public DbSet<Photo> Photos => Set<Photo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.CurrentSale)
                .WithMany()
                .HasForeignKey(e => e.CurrentSaleId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Photo>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Vehicle)
                .WithMany(e => e.Photos)
                .HasForeignKey(e => e.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<Prospect>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Ignore(e => e.BalanceDue);

            entity.HasOne(e => e.Vehicle)
                .WithMany(e => e.SalesHistory)
                .HasForeignKey(e => e.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Customer)
                .WithMany(e => e.Sales)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Fee>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Sale)
                .WithMany(e => e.Fees)
                .HasForeignKey(e => e.SaleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Sale)
                .WithMany(e => e.Payments)
                .HasForeignKey(e => e.SaleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<InsuranceProvider>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<RecurringJob>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<JobLog>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.RecurringJob)
                .WithMany()
                .HasForeignKey(e => e.RecurringJobId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
