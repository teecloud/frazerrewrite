using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrazerDealer.Infrastructure.Persistence.Migrations;

public partial class AddPhotos : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Photos",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Caption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                IsPrimary = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Photos", x => x.Id);
                table.ForeignKey(
                    name: "FK_Photos_Vehicles_VehicleId",
                    column: x => x.VehicleId,
                    principalTable: "Vehicles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Photos_VehicleId",
            table: "Photos",
            column: "VehicleId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Photos");
    }
}
