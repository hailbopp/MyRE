using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MyRE.Data.Migrations
{
    public partial class addprojectinstanceid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AppInstances_ParentInstanceAppInstanceId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ParentInstanceAppInstanceId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ParentInstanceAppInstanceId",
                table: "Projects");

            migrationBuilder.AddColumn<long>(
                name: "ParentInstanceId",
                table: "Projects",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ParentInstanceId",
                table: "Projects",
                column: "ParentInstanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AppInstances_ParentInstanceId",
                table: "Projects",
                column: "ParentInstanceId",
                principalTable: "AppInstances",
                principalColumn: "AppInstanceId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AppInstances_ParentInstanceId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ParentInstanceId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ParentInstanceId",
                table: "Projects");

            migrationBuilder.AddColumn<long>(
                name: "ParentInstanceAppInstanceId",
                table: "Projects",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ParentInstanceAppInstanceId",
                table: "Projects",
                column: "ParentInstanceAppInstanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AppInstances_ParentInstanceAppInstanceId",
                table: "Projects",
                column: "ParentInstanceAppInstanceId",
                principalTable: "AppInstances",
                principalColumn: "AppInstanceId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
