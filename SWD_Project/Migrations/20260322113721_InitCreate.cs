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
                name: "SkillCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillCategories", x => x.Id);
                });

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
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skills_SkillCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "SkillCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatBotLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    UserPrompt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeminiResponse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatBotLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatBotLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MentorCVs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MentorId = table.Column<int>(type: "int", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExperienceYears = table.Column<int>(type: "int", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false)
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
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MentorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requests_Users_MenteeId",
                        column: x => x.MenteeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_Users_MentorId",
                        column: x => x.MentorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VnPayTranId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MentorCVSkill",
                columns: table => new
                {
                    MentorCVsId = table.Column<int>(type: "int", nullable: false),
                    SkillsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentorCVSkill", x => new { x.MentorCVsId, x.SkillsId });
                    table.ForeignKey(
                        name: "FK_MentorCVSkill_MentorCVs_MentorCVsId",
                        column: x => x.MentorCVsId,
                        principalTable: "MentorCVs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MentorCVSkill_Skills_SkillsId",
                        column: x => x.SkillsId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "SkillCategories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Core programming languages", "Programming Languages" },
                    { 2, "Web and App Frameworks", "Frameworks" },
                    { 3, "Relational and NoSQL DBs", "Databases" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FullName", "IsBlocked", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, "mentee1@gmail.com", "Nguyen Van A", false, "e7618ce91a8c8d7c62754b8917a0f87c335d38c30380e6a410005db7ec7e294d", 1, "mentee1" },
                    { 2, "mentor1@gmail.com", "Tran Van B", false, "5bdfbd1dde4f476feb6da42e31143fa7caead053b7f3c7d7ce13b6e95a9fc3c3", 2, "mentor1" },
                    { 3, "admin@gmail.com", "System Administrator", false, "e86f78a8a3caf0b60d8e74e5942aa6d86dc150cd3c03338aef25b7d2d7e3acc7", 0, "admin" },
                    { 4, "mentor2@gmail.com", "Le Thi C", false, "55d94109ab97d00a56905da181814c150c8b96261953384289685ba385fcac9e", 2, "mentor2" }
                });

            migrationBuilder.InsertData(
                table: "MentorCVs",
                columns: new[] { "Id", "Bio", "ExperienceYears", "IsApproved", "MentorId" },
                values: new object[] { 1, "Senior .NET Developer with 5 years of experience.", 5, false, 2 });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "Content", "CreatedAt", "MenteeId", "MentorId", "Status", "Title" },
                values: new object[,]
                {
                    { 1, "I want to learn MVC", new DateTime(2026, 3, 22, 18, 37, 20, 962, DateTimeKind.Local).AddTicks(6362), 1, 2, 0, "Learn ASP.NET Core" },
                    { 2, "Need frontend mentor", new DateTime(2026, 3, 22, 18, 37, 20, 962, DateTimeKind.Local).AddTicks(6379), 1, 2, 1, "Learn React" },
                    { 3, "Database optimization", new DateTime(2026, 3, 22, 18, 37, 20, 962, DateTimeKind.Local).AddTicks(6380), 1, 2, 2, "Learn SQL" }
                });

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "Id", "CategoryId", "Description", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Ngôn ngữ C#", true, "C#" },
                    { 2, 1, "Ngôn ngữ Java", true, "Java" },
                    { 3, 2, "Web framework của Microsoft", true, "ASP.NET Core" },
                    { 4, 2, "Thư viện frontend UI", true, "React" },
                    { 5, 3, "Hệ quản trị CSDL quan hệ", true, "SQL Server" }
                });

            migrationBuilder.InsertData(
                table: "MentorCVSkill",
                columns: new[] { "MentorCVsId", "SkillsId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 3 },
                    { 1, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatBotLogs_UserId",
                table: "ChatBotLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorCVs_MentorId",
                table: "MentorCVs",
                column: "MentorId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorCVSkill_SkillsId",
                table: "MentorCVSkill",
                column: "SkillsId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_MenteeId",
                table: "Requests",
                column: "MenteeId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_MentorId",
                table: "Requests",
                column: "MentorId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_CategoryId",
                table: "Skills",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserId",
                table: "Transactions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatBotLogs");

            migrationBuilder.DropTable(
                name: "MentorCVSkill");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "MentorCVs");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "SkillCategories");
        }
    }
}
