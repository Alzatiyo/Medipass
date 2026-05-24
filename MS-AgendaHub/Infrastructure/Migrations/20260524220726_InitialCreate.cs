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
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DoctorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Specialty = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcedureCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Specialty = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InsuranceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "Id", "FullName", "Specialty" },
                values: new object[,]
                {
                    { new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa7"), "Dra. Sara Connor", "Medicina General" },
                    { new Guid("5ba85f64-5717-4562-b3fc-2c963f66afa8"), "Dr. Gregory House", "Cardiología" }
                });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "Id", "FullName", "InsuranceNumber" },
                values: new object[,]
                {
                    { new Guid("1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d"), "Juan Pérez", "INS-BASIC-ACTIVE" },
                    { new Guid("2b3c4d5e-6f7a-8b9c-0d1e-2f3a4b5c6d7e"), "Ana Gómez", "INS-PREMIUM-ACTIVE" },
                    { new Guid("3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f"), "Carlos Ruiz", "INS-ONCOLOGY-ACTIVE" },
                    { new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"), "Diego Rodríguez", "INS-001" },
                    { new Guid("4d5e6f7a-8b9c-0d1e-2f3a-4b5c6d7e8f9a"), "María López", "INS-INACTIVE" },
                    { new Guid("5e6f7a8b-9c0d-1e2f-3a4b-5c6d7e8f9a0b"), "Luis Silva", "INS-EXPIRED" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId_AppointmentDate",
                table: "Appointments",
                columns: new[] { "DoctorId", "AppointmentDate" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "Patients");
        }
    }
}
