using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrazerDealer.Infrastructure.Persistence.Migrations;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Customers",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Customers", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "InsuranceProviders",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                IsActive = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_InsuranceProviders", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "RecurringJobs",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CronExpression = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                LastRun = table.Column<DateTime>(type: "datetime2", nullable: true),
                IsActive = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RecurringJobs", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Vehicles",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Vin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                StockNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Year = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Make = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Trim = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                IsSold = table.Column<bool>(type: "bit", nullable: false),
                DateArrived = table.Column<DateTime>(type: "datetime2", nullable: true),
                DateSold = table.Column<DateTime>(type: "datetime2", nullable: true),
                CurrentSaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Vehicles", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "JobLogs",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                RecurringJobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                StartedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                CompletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                WasSuccessful = table.Column<bool>(type: "bit", nullable: false),
                Message = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_JobLogs", x => x.Id);
                table.ForeignKey(
                    name: "FK_JobLogs_RecurringJobs_RecurringJobId",
                    column: x => x.RecurringJobId,
                    principalTable: "RecurringJobs",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Sales",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Status = table.Column<int>(type: "int", nullable: false),
                CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                CompletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                FeesTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                PaymentsTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Sales", x => x.Id);
                table.ForeignKey(
                    name: "FK_Sales_Customers_CustomerId",
                    column: x => x.CustomerId,
                    principalTable: "Customers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Sales_Vehicles_VehicleId",
                    column: x => x.VehicleId,
                    principalTable: "Vehicles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Fees",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                SaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                IsRecurring = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Fees", x => x.Id);
                table.ForeignKey(
                    name: "FK_Fees_Sales_SaleId",
                    column: x => x.SaleId,
                    principalTable: "Sales",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Payments",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                SaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                CollectedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                Method = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Status = table.Column<int>(type: "int", nullable: false),
                ExternalReference = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Payments", x => x.Id);
                table.ForeignKey(
                    name: "FK_Payments_Sales_SaleId",
                    column: x => x.SaleId,
                    principalTable: "Sales",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Fees_SaleId",
            table: "Fees",
            column: "SaleId");

        migrationBuilder.CreateIndex(
            name: "IX_JobLogs_RecurringJobId",
            table: "JobLogs",
            column: "RecurringJobId");

        migrationBuilder.CreateIndex(
            name: "IX_Payments_SaleId",
            table: "Payments",
            column: "SaleId");

        migrationBuilder.CreateIndex(
            name: "IX_Sales_CustomerId",
            table: "Sales",
            column: "CustomerId");

        migrationBuilder.CreateIndex(
            name: "IX_Sales_VehicleId",
            table: "Sales",
            column: "VehicleId");

        migrationBuilder.CreateIndex(
            name: "IX_Vehicles_CurrentSaleId",
            table: "Vehicles",
            column: "CurrentSaleId");

        migrationBuilder.AddForeignKey(
            name: "FK_Vehicles_Sales_CurrentSaleId",
            table: "Vehicles",
            column: "CurrentSaleId",
            principalTable: "Sales",
            principalColumn: "Id",
            onDelete: ReferentialAction.SetNull);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Vehicles_Sales_CurrentSaleId')
            BEGIN
                ALTER TABLE [Vehicles] DROP CONSTRAINT [FK_Vehicles_Sales_CurrentSaleId];
            END
        """);

        migrationBuilder.Sql("DROP TABLE IF EXISTS [Fees];");
        migrationBuilder.Sql("DROP TABLE IF EXISTS [InsuranceProviders];");
        migrationBuilder.Sql("DROP TABLE IF EXISTS [JobLogs];");
        migrationBuilder.Sql("DROP TABLE IF EXISTS [Payments];");
        migrationBuilder.Sql("DROP TABLE IF EXISTS [RecurringJobs];");
        migrationBuilder.Sql("DROP TABLE IF EXISTS [Sales];");
        migrationBuilder.Sql("DROP TABLE IF EXISTS [Customers];");
        migrationBuilder.Sql("DROP TABLE IF EXISTS [Vehicles];");
    }
}
