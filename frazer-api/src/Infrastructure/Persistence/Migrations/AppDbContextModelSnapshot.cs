using System;
using System.Collections.Generic;
using FrazerDealer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FrazerDealer.Infrastructure.Persistence.Migrations;

[DbContext(typeof(AppDbContext))]
partial class AppDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "8.0.3")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

        modelBuilder.Entity("FrazerDealer.Domain.Entities.Customer", b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uniqueidentifier");

            b.Property<string>("Address")
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            b.Property<string>("City")
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            b.Property<string>("Email")
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            b.Property<string>("FirstName")
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            b.Property<string>("LastName")
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            b.Property<string>("Phone")
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            b.Property<string>("PostalCode")
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            b.Property<string>("State")
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            b.HasKey("Id");

            b.ToTable("Customers");
        });

        modelBuilder.Entity("FrazerDealer.Domain.Entities.Fee", b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uniqueidentifier");

            b.Property<string>("Code")
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            b.Property<string>("Description")
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            b.Property<bool>("IsRecurring")
                .HasColumnType("bit");

            b.Property<Guid>("SaleId")
                .HasColumnType("uniqueidentifier");

            b.Property<decimal>("Amount")
                .HasColumnType("decimal(18,2)");

            b.HasKey("Id");

            b.HasIndex("SaleId");

            b.ToTable("Fees");
        });

        modelBuilder.Entity("FrazerDealer.Domain.Entities.InsuranceProvider", b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uniqueidentifier");

            b.Property<string>("Email")
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            b.Property<bool>("IsActive")
                .HasColumnType("bit");

            b.Property<string>("Name")
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            b.Property<string>("Notes")
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            b.Property<string>("Phone")
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            b.HasKey("Id");

            b.ToTable("InsuranceProviders");
        });

        modelBuilder.Entity("FrazerDealer.Domain.Entities.JobLog", b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uniqueidentifier");

            b.Property<DateTime?>("CompletedOn")
                .HasColumnType("datetime2");

            b.Property<string>("Message")
                .HasColumnType("nvarchar(max)");

            b.Property<Guid>("RecurringJobId")
                .HasColumnType("uniqueidentifier");

            b.Property<DateTime>("StartedOn")
                .HasColumnType("datetime2");

            b.Property<bool>("WasSuccessful")
                .HasColumnType("bit");

            b.HasKey("Id");

            b.HasIndex("RecurringJobId");

            b.ToTable("JobLogs");
        });

        modelBuilder.Entity("FrazerDealer.Domain.Entities.Payment", b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uniqueidentifier");

            b.Property<decimal>("Amount")
                .HasColumnType("decimal(18,2)");

            b.Property<DateTime>("CollectedOn")
                .HasColumnType("datetime2");

            b.Property<string>("ExternalReference")
                .HasColumnType("nvarchar(max)");

            b.Property<string>("Method")
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            b.Property<Guid>("SaleId")
                .HasColumnType("uniqueidentifier");

            b.Property<int>("Status")
                .HasColumnType("int");

            b.HasKey("Id");

            b.HasIndex("SaleId");

            b.ToTable("Payments");
        });

        modelBuilder.Entity("FrazerDealer.Domain.Entities.Photo", b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uniqueidentifier");

            b.Property<string>("Caption")
                .HasColumnType("nvarchar(max)");

            b.Property<bool>("IsPrimary")
                .HasColumnType("bit");

            b.Property<string>("Url")
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            b.Property<Guid>("VehicleId")
                .HasColumnType("uniqueidentifier");

            b.HasKey("Id");

            b.HasIndex("VehicleId");

            b.ToTable("Photos");
        });

        modelBuilder.Entity("FrazerDealer.Domain.Entities.Prospect", b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uniqueidentifier");

            b.Property<string>("Email")
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            b.Property<string>("Name")
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            b.Property<string>("Phone")
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            b.HasKey("Id");

            b.ToTable("Prospects");

            b.Navigation("Vehicles");
        });

        modelBuilder.Entity("FrazerDealer.Domain.Entities.RecurringJob", b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uniqueidentifier");

            b.Property<string>("CronExpression")
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            b.Property<DateTime>("CreatedOn")
                .HasColumnType("datetime2");

            b.Property<bool>("IsActive")
                .HasColumnType("bit");

            b.Property<DateTime?>("LastRun")
                .HasColumnType("datetime2");

            b.Property<string>("Name")
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            b.HasKey("Id");

            b.ToTable("RecurringJobs");
        });

        modelBuilder.Entity("FrazerDealer.Domain.Entities.Sale", b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uniqueidentifier");

            b.Property<DateTime?>("CompletedOn")
                .HasColumnType("datetime2");

            b.Property<Guid>("CustomerId")
                .HasColumnType("uniqueidentifier");

            b.Property<decimal>("FeesTotal")
                .HasColumnType("decimal(18,2)");

            b.Property<decimal>("PaymentsTotal")
                .HasColumnType("decimal(18,2)");

            b.Property<DateTime>("CreatedOn")
                .HasColumnType("datetime2");

            b.Property<int>("Status")
                .HasColumnType("int");

            b.Property<decimal>("Subtotal")
                .HasColumnType("decimal(18,2)");

            b.Property<Guid>("VehicleId")
                .HasColumnType("uniqueidentifier");

            b.HasKey("Id");

            b.HasIndex("CustomerId");

            b.HasIndex("VehicleId");

            b.ToTable("Sales");
        });

        modelBuilder.Entity("FrazerDealer.Domain.Entities.Vehicle", b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uniqueidentifier");

            b.Property<decimal>("Cost")
                .HasColumnType("decimal(18,2)");

            b.Property<Guid?>("CurrentSaleId")
                .HasColumnType("uniqueidentifier");

            b.Property<DateTime?>("DateArrived")
                .HasColumnType("datetime2");

            b.Property<DateTime?>("DateSold")
                .HasColumnType("datetime2");

            b.Property<bool>("IsSold")
                .HasColumnType("bit");

            b.Property<string>("Make")
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnType("nvarchar(128)");

            b.Property<string>("Model")
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnType("nvarchar(128)");

            b.Property<decimal>("Price")
                .HasColumnType("decimal(18,2)");

            b.Property<string>("StockNumber")
                .IsRequired()
                .HasMaxLength(64)
                .HasColumnType("nvarchar(64)");

            b.Property<string>("Trim")
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnType("nvarchar(128)");

            b.Property<string>("Vin")
                .IsRequired()
                .HasMaxLength(17)
                .HasColumnType("nvarchar(17)");

            b.Property<string>("Year")
                .IsRequired()
                .HasMaxLength(4)
                .HasColumnType("nvarchar(4)");

            b.HasKey("Id");

            b.HasIndex("CurrentSaleId");

            b.ToTable("Vehicles");
        });

        modelBuilder.Entity("FrazerDealer.Domain.Entities.ProspectVehicle", b =>
        {
            b.Property<Guid>("ProspectId")
                .HasColumnType("uniqueidentifier");

            b.Property<Guid>("VehicleId")
                .HasColumnType("uniqueidentifier");

            b.HasKey("ProspectId", "VehicleId");

            b.HasIndex("VehicleId");

            b.ToTable("ProspectVehicle", (string)null);
        });

        modelBuilder.Entity("FrazerDealer.Domain.Entities.Fee", b =>
        {
            b.HasOne("FrazerDealer.Domain.Entities.Sale", "Sale")
                .WithMany("Fees")
                .HasForeignKey("SaleId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("Sale");
        });

        modelBuilder.Entity("FrazerDealer.Domain.Entities.JobLog", b =>
        {
            b.HasOne("FrazerDealer.Domain.Entities.RecurringJob", "RecurringJob")
                .WithMany()
                .HasForeignKey("RecurringJobId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("RecurringJob");
        });

        modelBuilder.Entity("FrazerDealer.Domain.Entities.Payment", b =>
        {
            b.HasOne("FrazerDealer.Domain.Entities.Sale", "Sale")
                .WithMany("Payments")
                .HasForeignKey("SaleId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("Sale");
        });

        modelBuilder.Entity("FrazerDealer.Domain.Entities.Sale", b =>
        {
            b.HasOne("FrazerDealer.Domain.Entities.Customer", "Customer")
                .WithMany("Sales")
                .HasForeignKey("CustomerId")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            b.HasOne("FrazerDealer.Domain.Entities.Vehicle", "Vehicle")
                .WithMany("SalesHistory")
                .HasForeignKey("VehicleId")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            b.Navigation("Customer");

            b.Navigation("Vehicle");
        });

        modelBuilder.Entity("FrazerDealer.Domain.Entities.Photo", b =>
        {
            b.HasOne("FrazerDealer.Domain.Entities.Vehicle", "Vehicle")
                .WithMany("Photos")
                .HasForeignKey("VehicleId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("Vehicle");
        });

        modelBuilder.Entity("FrazerDealer.Domain.Entities.Vehicle", b =>
        {
            b.HasOne("FrazerDealer.Domain.Entities.Sale", "CurrentSale")
                .WithMany()
                .HasForeignKey("CurrentSaleId")
                .OnDelete(DeleteBehavior.SetNull);

            b.HasMany("FrazerDealer.Domain.Entities.Prospect", "Prospects")
                .WithMany("Vehicles")
                .UsingEntity(
                    typeof(FrazerDealer.Domain.Entities.ProspectVehicle),
                    r => r.HasOne("FrazerDealer.Domain.Entities.Prospect", "Prospect")
                        .WithMany()
                        .HasForeignKey("ProspectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired(),
                    l => l.HasOne("FrazerDealer.Domain.Entities.Vehicle", "Vehicle")
                        .WithMany()
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired(),
                    j =>
                    {
                        j.HasKey("ProspectId", "VehicleId");
                        j.ToTable("ProspectVehicle");
                    });

            b.Navigation("CurrentSale");
        });

        modelBuilder.Entity("FrazerDealer.Domain.Entities.ProspectVehicle", b =>
        {
            b.HasOne("FrazerDealer.Domain.Entities.Prospect", "Prospect")
                .WithMany()
                .HasForeignKey("ProspectId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.HasOne("FrazerDealer.Domain.Entities.Vehicle", "Vehicle")
                .WithMany()
                .HasForeignKey("VehicleId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("Prospect");

            b.Navigation("Vehicle");
        });

        modelBuilder.Entity("FrazerDealer.Domain.Entities.Customer", b =>
        {
            b.Navigation("Sales");
        });

        modelBuilder.Entity("FrazerDealer.Domain.Entities.Sale", b =>
        {
            b.Navigation("Fees");

            b.Navigation("Payments");
        });

        modelBuilder.Entity("FrazerDealer.Domain.Entities.Vehicle", b =>
        {
            b.Navigation("Photos");

            b.Navigation("Prospects");

            b.Navigation("SalesHistory");
        });
#pragma warning restore 612, 618
    }
}
