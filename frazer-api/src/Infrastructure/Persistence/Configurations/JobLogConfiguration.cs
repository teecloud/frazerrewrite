using FrazerDealer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FrazerDealer.Infrastructure.Persistence.Configurations;

public class JobLogConfiguration : IEntityTypeConfiguration<JobLog>
{
    public void Configure(EntityTypeBuilder<JobLog> builder)
    {
        builder.HasKey(j => j.Id);
        builder.Property(j => j.Message).HasMaxLength(1024);

        builder.HasOne(j => j.RecurringJob)
            .WithMany()
            .HasForeignKey(j => j.RecurringJobId);
    }
}
