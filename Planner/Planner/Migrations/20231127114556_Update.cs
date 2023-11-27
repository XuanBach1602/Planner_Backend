using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planner.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UploadFiles_TemporaryWorkTasks_TemporaryWorkTaskId",
                table: "UploadFiles");

            migrationBuilder.DropIndex(
                name: "IX_UploadFiles_TemporaryWorkTaskId",
                table: "UploadFiles");

            migrationBuilder.DropColumn(
                name: "TemporaryWorkTaskId",
                table: "UploadFiles");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "TemporaryWorkTasks");

            migrationBuilder.DropColumn(
                name: "CreatedUserID",
                table: "TemporaryWorkTasks");

            migrationBuilder.DropColumn(
                name: "PlanID",
                table: "TemporaryWorkTasks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TemporaryWorkTaskId",
                table: "UploadFiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryID",
                table: "TemporaryWorkTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserID",
                table: "TemporaryWorkTasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PlanID",
                table: "TemporaryWorkTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
    }
}
