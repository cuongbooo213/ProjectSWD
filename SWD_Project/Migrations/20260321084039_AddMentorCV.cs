using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWD_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddMentorCV : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MentorCVs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MentorId = table.Column<int>(type: "int", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Experience = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Skills = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentorCVs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MentorCVs_Users_MentorId",
                        column: x => x.MentorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MentorCVs",
                columns: new[] { "Id", "Bio", "Experience", "MentorId", "Skills" },
                values: new object[] { 1, "Senior .NET Developer with 5 years of experience.", "FPT Software (2020-2023), VNG (2023-Present)", 2, "C#, ASP.NET Core, SQL Server, Azure" });

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 21, 15, 40, 39, 14, DateTimeKind.Local).AddTicks(7663));

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 21, 15, 40, 39, 14, DateTimeKind.Local).AddTicks(7677));

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 21, 15, 40, 39, 14, DateTimeKind.Local).AddTicks(7679));

            migrationBuilder.CreateIndex(
                name: "IX_MentorCVs_MentorId",
                table: "MentorCVs",
                column: "MentorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MentorCVs");

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 15, 27, 37, 723, DateTimeKind.Local).AddTicks(2782));

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 15, 27, 37, 723, DateTimeKind.Local).AddTicks(2795));

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 15, 27, 37, 723, DateTimeKind.Local).AddTicks(2797));
        }
    }
}
