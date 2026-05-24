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
                    { new Guid("1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d"), new DateTime(2031, 5, 24, 22, 7, 44, 251, DateTimeKind.Utc).AddTicks(262), "INS-BASIC-ACTIVE", true, "Juan Pérez", "Basic" },
                    { new Guid("2b3c4d5e-6f7a-8b9c-0d1e-2f3a4b5c6d7e"), new DateTime(2031, 5, 24, 22, 7, 44, 251, DateTimeKind.Utc).AddTicks(265), "INS-PREMIUM-ACTIVE", true, "Ana Gómez", "Premium" },
                    { new Guid("3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f"), new DateTime(2031, 5, 24, 22, 7, 44, 251, DateTimeKind.Utc).AddTicks(267), "INS-ONCOLOGY-ACTIVE", true, "Carlos Ruiz", "Oncology" },
                    { new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"), new DateTime(2036, 5, 24, 22, 7, 44, 251, DateTimeKind.Utc).AddTicks(252), "INS-001", true, "Diego Rodríguez", "Basic" },
                    { new Guid("4d5e6f7a-8b9c-0d1e-2f3a-4b5c6d7e8f9a"), new DateTime(2028, 5, 24, 22, 7, 44, 251, DateTimeKind.Utc).AddTicks(269), "INS-INACTIVE", false, "María López", "Premium" },
                    { new Guid("5e6f7a8b-9c0d-1e2f-3a4b-5c6d7e8f9a0b"), new DateTime(2025, 5, 24, 22, 7, 44, 251, DateTimeKind.Utc).AddTicks(271), "INS-EXPIRED", true, "Luis Silva", "Basic" }
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
