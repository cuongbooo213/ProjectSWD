using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SWD_Project.Migrations
{
    /// <inheritdoc />
    public partial class InitCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenteeId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requests_Users_MenteeId",
                        column: x => x.MenteeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FullName", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, "mentee1@gmail.com", "Nguyen Van A", "123456", 1, "mentee1" },
                    { 2, "mentor@gmail.com", "Tran Van B", "123456", 2, "mentor1" }
                });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "Content", "CreatedAt", "MenteeId", "Status", "Title" },
                values: new object[,]
                {
                    { 1, "I want to learn MVC", new DateTime(2026, 3, 18, 10, 18, 13, 255, DateTimeKind.Local).AddTicks(8027), 1, 0, "Learn ASP.NET Core" },
                    { 2, "Need frontend mentor", new DateTime(2026, 3, 18, 10, 18, 13, 255, DateTimeKind.Local).AddTicks(8039), 1, 1, "Learn React" },
                    { 3, "Database optimization", new DateTime(2026, 3, 18, 10, 18, 13, 255, DateTimeKind.Local).AddTicks(8040), 1, 2, "Learn SQL" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_MenteeId",
                table: "Requests",
                column: "MenteeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
