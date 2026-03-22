using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWD_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddMentorToRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Users_MenteeId",
                table: "Requests");

            migrationBuilder.AddColumn<int>(
                name: "MentorId",
                table: "Requests",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "MentorId" },
                values: new object[] { new DateTime(2026, 3, 18, 15, 27, 37, 723, DateTimeKind.Local).AddTicks(2782), 2 });

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "MentorId" },
                values: new object[] { new DateTime(2026, 3, 18, 15, 27, 37, 723, DateTimeKind.Local).AddTicks(2795), 2 });

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "MentorId" },
                values: new object[] { new DateTime(2026, 3, 18, 15, 27, 37, 723, DateTimeKind.Local).AddTicks(2797), 2 });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_MentorId",
                table: "Requests",
                column: "MentorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Users_MenteeId",
                table: "Requests",
                column: "MenteeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Users_MentorId",
                table: "Requests",
                column: "MentorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Users_MenteeId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Users_MentorId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_MentorId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "MentorId",
                table: "Requests");

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 10, 18, 13, 255, DateTimeKind.Local).AddTicks(8027));

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 10, 18, 13, 255, DateTimeKind.Local).AddTicks(8039));

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 10, 18, 13, 255, DateTimeKind.Local).AddTicks(8040));

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Users_MenteeId",
                table: "Requests",
                column: "MenteeId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
