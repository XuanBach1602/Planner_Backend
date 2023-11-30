using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planner.Migrations
{
    /// <inheritdoc />
    public partial class addWorkTaskOriginToDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_WorkTasks_OriginId",
                table: "WorkTasks",
                column: "OriginId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkTasks_WorkTasks_OriginId",
                table: "WorkTasks",
                column: "OriginId",
                principalTable: "WorkTasks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkTasks_WorkTasks_OriginId",
                table: "WorkTasks");

            migrationBuilder.DropIndex(
                name: "IX_WorkTasks_OriginId",
                table: "WorkTasks");
        }
    }
}
