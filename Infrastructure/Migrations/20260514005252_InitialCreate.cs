using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlightPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DroneId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OriginZone = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DestinationZone = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AltitudeMeters = table.Column<double>(type: "float", nullable: false),
                    ZoneType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WindSpeedKmh = table.Column<double>(type: "float", nullable: false),
                    IsNoFlyZoneViolation = table.Column<bool>(type: "bit", nullable: false),
                    RejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthorizedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveryProofImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NoFlyZones",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ZoneName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ZoneType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinLatitude = table.Column<double>(type: "float", nullable: false),
                    MaxLatitude = table.Column<double>(type: "float", nullable: false),
                    MinLongitude = table.Column<double>(type: "float", nullable: false),
                    MaxLongitude = table.Column<double>(type: "float", nullable: false),
                    MinAltitudeMeters = table.Column<double>(type: "float", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoFlyZones", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlightPlans");

            migrationBuilder.DropTable(
                name: "NoFlyZones");
        }
    }
}
