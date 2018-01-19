using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MyRE.Data.Migrations
{
    public partial class discriminatorexplicit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routines_Block_BlockId",
                table: "Routines");

            migrationBuilder.DropForeignKey(
                name: "FK_Routines_Projects_ProjectId",
                table: "Routines");

            migrationBuilder.DropForeignKey(
                name: "FK_Statements_Expressions_ExpressionToEvaluateExpressionId",
                table: "Statements");

            migrationBuilder.DropIndex(
                name: "IX_Statements_ExpressionToEvaluateExpressionId",
                table: "Statements");

            migrationBuilder.DropColumn(
                name: "ExpressionToEvaluateExpressionId",
                table: "Statements");

            migrationBuilder.AddColumn<Guid>(
                name: "ExpressionToEvaluateId",
                table: "Statements",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ProjectId",
                table: "Routines",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "BlockId",
                table: "Routines",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Statements_ExpressionToEvaluateId",
                table: "Statements",
                column: "ExpressionToEvaluateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Routines_Block_BlockId",
                table: "Routines",
                column: "BlockId",
                principalTable: "Block",
                principalColumn: "BlockId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Routines_Projects_ProjectId",
                table: "Routines",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Statements_Expressions_ExpressionToEvaluateId",
                table: "Statements",
                column: "ExpressionToEvaluateId",
                principalTable: "Expressions",
                principalColumn: "ExpressionId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routines_Block_BlockId",
                table: "Routines");

            migrationBuilder.DropForeignKey(
                name: "FK_Routines_Projects_ProjectId",
                table: "Routines");

            migrationBuilder.DropForeignKey(
                name: "FK_Statements_Expressions_ExpressionToEvaluateId",
                table: "Statements");

            migrationBuilder.DropIndex(
                name: "IX_Statements_ExpressionToEvaluateId",
                table: "Statements");

            migrationBuilder.DropColumn(
                name: "ExpressionToEvaluateId",
                table: "Statements");

            migrationBuilder.AddColumn<Guid>(
                name: "ExpressionToEvaluateExpressionId",
                table: "Statements",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ProjectId",
                table: "Routines",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "BlockId",
                table: "Routines",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.CreateIndex(
                name: "IX_Statements_ExpressionToEvaluateExpressionId",
                table: "Statements",
                column: "ExpressionToEvaluateExpressionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Routines_Block_BlockId",
                table: "Routines",
                column: "BlockId",
                principalTable: "Block",
                principalColumn: "BlockId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Routines_Projects_ProjectId",
                table: "Routines",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Statements_Expressions_ExpressionToEvaluateExpressionId",
                table: "Statements",
                column: "ExpressionToEvaluateExpressionId",
                principalTable: "Expressions",
                principalColumn: "ExpressionId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
