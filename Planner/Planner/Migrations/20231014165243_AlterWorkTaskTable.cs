using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planner.Migrations
{
    /// <inheritdoc />
    public partial class AlterWorkTaskTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_WorkTasks_PlanID",
                table: "WorkTasks",
                column: "PlanID");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkTasks_Plans_PlanID",
                table: "WorkTasks",
                column: "PlanID",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkTasks_Plans_PlanID",
                table: "WorkTasks");

            migrationBuilder.DropIndex(
                name: "IX_WorkTasks_PlanID",
                table: "WorkTasks");
        }
    }
}
