using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planner.Migrations
{
    /// <inheritdoc />
    public partial class AddCompletedUserToTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompletedUserId",
                table: "WorkTasks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkTasks_CompletedUserId",
                table: "WorkTasks",
                column: "CompletedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkTasks_AspNetUsers_CompletedUserId",
                table: "WorkTasks",
                column: "CompletedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkTasks_AspNetUsers_CompletedUserId",
                table: "WorkTasks");

            migrationBuilder.DropIndex(
                name: "IX_WorkTasks_CompletedUserId",
                table: "WorkTasks");

            migrationBuilder.DropColumn(
                name: "CompletedUserId",
                table: "WorkTasks");
        }
    }
}
