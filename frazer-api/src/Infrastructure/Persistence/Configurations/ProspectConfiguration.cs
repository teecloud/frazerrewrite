using System.Collections.Generic;
using FrazerDealer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrazerDealer.Infrastructure.Persistence.Configurations;

public class ProspectConfiguration : IEntityTypeConfiguration<Prospect>
{
    public void Configure(EntityTypeBuilder<Prospect> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasMany(p => p.Vehicles)
            .WithMany(v => v.Prospects)
            .UsingEntity<Dictionary<string, object>>(
                "ProspectVehicle",
                j => j.HasOne<Vehicle>()
                    .WithMany()
                    .HasForeignKey("VehicleId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<Prospect>()
                    .WithMany()
                    .HasForeignKey("ProspectId")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("ProspectId", "VehicleId");
                    j.ToTable("ProspectVehicle");
                });
    }
}
