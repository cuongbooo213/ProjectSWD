using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWD_Project.Migrations
{
    /// <inheritdoc />
    public partial class FixSkillCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1000);

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 22, 15, 17, 57, 952, DateTimeKind.Local).AddTicks(1690));

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 22, 15, 17, 57, 952, DateTimeKind.Local).AddTicks(1700));

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 22, 15, 17, 57, 952, DateTimeKind.Local).AddTicks(1702));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 22, 13, 54, 38, 663, DateTimeKind.Local).AddTicks(4505));

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 22, 13, 54, 38, 663, DateTimeKind.Local).AddTicks(4520));

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 22, 13, 54, 38, 663, DateTimeKind.Local).AddTicks(4522));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FullName", "PasswordHash", "Role", "Username" },
                values: new object[] { 1000, "admin@happyprogramming.com", "Admin HP", "admin123", 0, "adminhp123" });
        }
    }
}
