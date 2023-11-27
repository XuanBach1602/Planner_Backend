using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planner.Migrations
{
    /// <inheritdoc />
    public partial class AddTemporaryWorkTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TemporaryWorkTaskId",
                table: "UploadFiles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TemporaryWorkTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkTaskId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPrivate = table.Column<bool>(type: "bit", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    PlanID = table.Column<int>(type: "int", nullable: false),
                    CreatedUserID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssignedUserID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedUserID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemporaryWorkTasks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UploadFiles_TemporaryWorkTaskId",
                table: "UploadFiles",
                column: "TemporaryWorkTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_UploadFiles_TemporaryWorkTasks_TemporaryWorkTaskId",
                table: "UploadFiles",
                column: "TemporaryWorkTaskId",
                principalTable: "TemporaryWorkTasks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UploadFiles_TemporaryWorkTasks_TemporaryWorkTaskId",
                table: "UploadFiles");

            migrationBuilder.DropTable(
                name: "TemporaryWorkTasks");

            migrationBuilder.DropIndex(
                name: "IX_UploadFiles_TemporaryWorkTaskId",
                table: "UploadFiles");

            migrationBuilder.DropColumn(
                name: "TemporaryWorkTaskId",
                table: "UploadFiles");
        }
    }
}
