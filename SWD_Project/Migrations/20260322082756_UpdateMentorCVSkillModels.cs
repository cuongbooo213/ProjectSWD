using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWD_Project.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMentorCVSkillModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 22, 15, 27, 56, 387, DateTimeKind.Local).AddTicks(3987));

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 22, 15, 27, 56, 387, DateTimeKind.Local).AddTicks(4014));

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 22, 15, 27, 56, 387, DateTimeKind.Local).AddTicks(4015));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
