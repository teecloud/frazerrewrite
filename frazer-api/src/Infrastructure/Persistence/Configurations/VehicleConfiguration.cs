using FrazerDealer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrazerDealer.Infrastructure.Persistence.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.HasKey(v => v.Id);
        builder.Property(v => v.StockNumber).HasMaxLength(64);
        builder.Property(v => v.Vin).HasMaxLength(17);
        builder.Property(v => v.Make).HasMaxLength(128);
        builder.Property(v => v.Model).HasMaxLength(128);
        builder.Property(v => v.Trim).HasMaxLength(128);
        builder.Property(v => v.Year).HasMaxLength(4);
        builder.Property(v => v.Price).HasColumnType("decimal(18,2)");
        builder.Property(v => v.Cost).HasColumnType("decimal(18,2)");

        builder.HasMany(v => v.SalesHistory)
            .WithOne(s => s.Vehicle)
            .HasForeignKey(s => s.VehicleId);

        builder.HasMany(v => v.Prospects)
            .WithMany(p => p.Vehicles)
            .UsingEntity(
                joinEntityType: typeof(ProspectVehicle),
                configureRight: j => j.HasOne(nameof(ProspectVehicle.Prospect))
                    .WithMany(nameof(Prospect.ProspectVehicles))
                    .HasForeignKey(nameof(ProspectVehicle.ProspectId)),
                configureLeft: j => j.HasOne(nameof(ProspectVehicle.Vehicle))
                    .WithMany(nameof(Vehicle.ProspectVehicles))
                    .HasForeignKey(nameof(ProspectVehicle.VehicleId)),
                configureJoinEntityType: j =>
                {
                    j.HasKey(nameof(ProspectVehicle.ProspectId), nameof(ProspectVehicle.VehicleId));
                    j.ToTable("ProspectVehicle");
                });
    }
}
