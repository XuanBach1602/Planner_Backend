﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planner.Migrations
{
    /// <inheritdoc />
    public partial class AlterDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CreatedUserID",
                table: "WorkTasks",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AssignedUserID",
                table: "WorkTasks",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserPlans",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Plans",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkTasks_AssignedUserID",
                table: "WorkTasks",
                column: "AssignedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTasks_CreatedUserID",
                table: "WorkTasks",
                column: "CreatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlans_PlanId",
                table: "UserPlans",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlans_UserId",
                table: "UserPlans",
                column: "UserId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_UserPlans_AspNetUsers_UserId",
                table: "UserPlans",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPlans_Plans_PlanId",
                table: "UserPlans",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkTasks_AspNetUsers_AssignedUserID",
                table: "WorkTasks",
                column: "AssignedUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkTasks_AspNetUsers_CreatedUserID",
                table: "WorkTasks",
                column: "CreatedUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Plans_AspNetUsers_UserId",
                table: "Plans");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPlans_AspNetUsers_UserId",
                table: "UserPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPlans_Plans_PlanId",
                table: "UserPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkTasks_AspNetUsers_AssignedUserID",
                table: "WorkTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkTasks_AspNetUsers_CreatedUserID",
                table: "WorkTasks");

            migrationBuilder.DropIndex(
                name: "IX_WorkTasks_AssignedUserID",
                table: "WorkTasks");

            migrationBuilder.DropIndex(
                name: "IX_WorkTasks_CreatedUserID",
                table: "WorkTasks");

            migrationBuilder.DropIndex(
                name: "IX_UserPlans_PlanId",
                table: "UserPlans");

            migrationBuilder.DropIndex(
                name: "IX_UserPlans_UserId",
                table: "UserPlans");

            migrationBuilder.DropIndex(
                name: "IX_Plans_UserId",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Plans");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedUserID",
                table: "WorkTasks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "AssignedUserID",
                table: "WorkTasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserPlans",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
