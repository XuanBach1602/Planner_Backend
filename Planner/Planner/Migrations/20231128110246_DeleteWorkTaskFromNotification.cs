using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planner.Migrations
{
    /// <inheritdoc />
    public partial class DeleteWorkTaskFromNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_WorkTasks_WorkTaskId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_WorkTaskId",
                table: "Notifications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Notifications_WorkTaskId",
                table: "Notifications",
                column: "WorkTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_WorkTasks_WorkTaskId",
                table: "Notifications",
                column: "WorkTaskId",
                principalTable: "WorkTasks",
                principalColumn: "Id");
        }
    }
}
