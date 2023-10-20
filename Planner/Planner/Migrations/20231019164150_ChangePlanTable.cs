using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planner.Migrations
{
    /// <inheritdoc />
    public partial class ChangePlanTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plans_AspNetUsers_UserId",
                table: "Plans");

            migrationBuilder.DropIndex(
                name: "IX_Plans_UserId",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Plans");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Plans",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Plans_UserId",
                table: "Plans",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Plans_AspNetUsers_UserId",
                table: "Plans",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
