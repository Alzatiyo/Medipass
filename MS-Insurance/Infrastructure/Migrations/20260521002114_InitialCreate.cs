using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InsurancePolicies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InsuranceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PatientName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Plan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsurancePolicies", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "InsurancePolicies",
                columns: new[] { "Id", "ExpirationDate", "InsuranceNumber", "IsActive", "PatientName", "Plan" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2031, 5, 21, 0, 21, 14, 412, DateTimeKind.Utc).AddTicks(979), "INS-BASIC-ACTIVE", true, "Juan Pérez", "Basic" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2031, 5, 21, 0, 21, 14, 412, DateTimeKind.Utc).AddTicks(981), "INS-PREMIUM-ACTIVE", true, "Ana Gómez", "Premium" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2031, 5, 21, 0, 21, 14, 412, DateTimeKind.Utc).AddTicks(984), "INS-ONCOLOGY-ACTIVE", true, "Carlos Ruiz", "Oncology" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2028, 5, 21, 0, 21, 14, 412, DateTimeKind.Utc).AddTicks(985), "INS-INACTIVE", false, "María López", "Premium" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(2025, 5, 21, 0, 21, 14, 412, DateTimeKind.Utc).AddTicks(987), "INS-EXPIRED", true, "Luis Silva", "Basic" },
                    { new Guid("99999999-9999-9999-9999-999999999999"), new DateTime(2036, 5, 21, 0, 21, 14, 412, DateTimeKind.Utc).AddTicks(962), "INS-001", true, "Diego Rodríguez", "Basic" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_InsurancePolicies_InsuranceNumber",
                table: "InsurancePolicies",
                column: "InsuranceNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InsurancePolicies");
        }
    }
}
