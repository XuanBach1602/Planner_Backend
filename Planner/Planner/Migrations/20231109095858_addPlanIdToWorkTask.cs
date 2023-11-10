using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planner.Migrations
{
    /// <inheritdoc />
    public partial class addPlanIdToWorkTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlanId",
                table: "WorkTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "WorkTasks");
        }
    }
}
