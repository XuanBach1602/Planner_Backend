using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planner.Migrations
{
    /// <inheritdoc />
    public partial class AddPlanIdToNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlanId",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_PlanId",
                table: "Notifications",
                column: "PlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Plans_PlanId",
                table: "Notifications",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Plans_PlanId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_PlanId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "Notifications");
        }
    }
}
