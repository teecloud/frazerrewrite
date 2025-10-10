using FrazerDealer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrazerDealer.Infrastructure.Persistence.Configurations;

public class RecurringJobConfiguration : IEntityTypeConfiguration<RecurringJob>
{
    public void Configure(EntityTypeBuilder<RecurringJob> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Name).HasMaxLength(128);
        builder.Property(r => r.CronExpression).HasMaxLength(64);
    }
}
