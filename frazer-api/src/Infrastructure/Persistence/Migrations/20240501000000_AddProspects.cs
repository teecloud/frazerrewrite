using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrazerDealer.Infrastructure.Persistence.Migrations;

public partial class AddProspects : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Prospects",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Phone = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Prospects", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ProspectVehicle",
            columns: table => new
            {
                ProspectsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                VehiclesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProspectVehicle", x => new { x.ProspectsId, x.VehiclesId });
                table.ForeignKey(
                    name: "FK_ProspectVehicle_Prospects_ProspectsId",
                    column: x => x.ProspectsId,
                    principalTable: "Prospects",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_ProspectVehicle_Vehicles_VehiclesId",
                    column: x => x.VehiclesId,
                    principalTable: "Vehicles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ProspectVehicle_VehiclesId",
            table: "ProspectVehicle",
            column: "VehiclesId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ProspectVehicle");

        migrationBuilder.DropTable(
            name: "Prospects");
    }
}
