using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planner.Migrations
{
    /// <inheritdoc />
    public partial class changeWorkTaskModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "WorkTasks",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "WorkTasks");
        }
    }
}
