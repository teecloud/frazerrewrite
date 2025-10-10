using FrazerDealer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrazerDealer.Infrastructure.Persistence.Configurations;

public class FeeConfiguration : IEntityTypeConfiguration<Fee>
{
    public void Configure(EntityTypeBuilder<Fee> builder)
    {
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Code).HasMaxLength(64);
        builder.Property(f => f.Description).HasMaxLength(256);
        builder.Property(f => f.Amount).HasColumnType("numeric(18,2)");
    }
}
