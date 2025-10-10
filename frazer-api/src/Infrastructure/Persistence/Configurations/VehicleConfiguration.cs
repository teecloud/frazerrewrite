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

        builder.HasMany(v => v.SalesHistory)
            .WithOne(s => s.Vehicle)
            .HasForeignKey(s => s.VehicleId);
    }
}
