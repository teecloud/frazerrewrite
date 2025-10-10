using FrazerDealer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrazerDealer.Infrastructure.Persistence.Configurations;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Subtotal).HasColumnType("numeric(18,2)");
        builder.Property(s => s.FeesTotal).HasColumnType("numeric(18,2)");
        builder.Property(s => s.PaymentsTotal).HasColumnType("numeric(18,2)");

        builder.HasMany(s => s.Fees)
            .WithOne(f => f.Sale)
            .HasForeignKey(f => f.SaleId);

        builder.HasMany(s => s.Payments)
            .WithOne(p => p.Sale)
            .HasForeignKey(p => p.SaleId);
    }
}
