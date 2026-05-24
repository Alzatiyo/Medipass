using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedData_DoctorsAndPatients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Juan Pérez", "INS-BASIC-ACTIVE" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Ana Gómez", "INS-PREMIUM-ACTIVE" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Carlos Ruiz", "INS-ONCOLOGY-ACTIVE" },
                    { new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"), "Diego Rodríguez", "INS-001" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa7"));

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: new Guid("5ba85f64-5717-4562-b3fc-2c963f66afa8"));

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "Id",
                keyValue: new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"));
        }
    }
}
