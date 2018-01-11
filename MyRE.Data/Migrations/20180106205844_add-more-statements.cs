using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MyRE.Data.Migrations
{
    public partial class addmorestatements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Statements_Expressions_ConditionExpressionId",
                table: "Statements");

            migrationBuilder.RenameColumn(
                name: "ConditionExpressionId",
                table: "Statements",
                newName: "WhileStatement_ConditionExpressionId");

            migrationBuilder.RenameIndex(
                name: "IX_Statements_ConditionExpressionId",
                table: "Statements",
                newName: "IX_Statements_WhileStatement_ConditionExpressionId");

            migrationBuilder.AddColumn<long>(
                name: "ExpressionToEvaluateExpressionId",
                table: "Statements",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ConditionExpressionId",
                table: "Statements",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ValueExpressionId",
                table: "Statements",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VariableName",
                table: "Statements",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VariableDefinitionStatement_VariableName",
                table: "Statements",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VariableType",
                table: "Statements",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WhileStatementStatementId",
                table: "BlockStatement",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Statements_ExpressionToEvaluateExpressionId",
                table: "Statements",
                column: "ExpressionToEvaluateExpressionId");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_ConditionExpressionId",
                table: "Statements",
                column: "ConditionExpressionId");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_ValueExpressionId",
                table: "Statements",
                column: "ValueExpressionId");

            migrationBuilder.CreateIndex(
                name: "IX_BlockStatement_WhileStatementStatementId",
                table: "BlockStatement",
                column: "WhileStatementStatementId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlockStatement_Statements_WhileStatementStatementId",
                table: "BlockStatement",
                column: "WhileStatementStatementId",
                principalTable: "Statements",
                principalColumn: "StatementId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Statements_Expressions_ExpressionToEvaluateExpressionId",
                table: "Statements",
                column: "ExpressionToEvaluateExpressionId",
                principalTable: "Expressions",
                principalColumn: "ExpressionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Statements_Expressions_ConditionExpressionId",
                table: "Statements",
                column: "ConditionExpressionId",
                principalTable: "Expressions",
                principalColumn: "ExpressionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Statements_Expressions_ValueExpressionId",
                table: "Statements",
                column: "ValueExpressionId",
                principalTable: "Expressions",
                principalColumn: "ExpressionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Statements_Expressions_WhileStatement_ConditionExpressionId",
                table: "Statements",
                column: "WhileStatement_ConditionExpressionId",
                principalTable: "Expressions",
                principalColumn: "ExpressionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlockStatement_Statements_WhileStatementStatementId",
                table: "BlockStatement");

            migrationBuilder.DropForeignKey(
                name: "FK_Statements_Expressions_ExpressionToEvaluateExpressionId",
                table: "Statements");

            migrationBuilder.DropForeignKey(
                name: "FK_Statements_Expressions_ConditionExpressionId",
                table: "Statements");

            migrationBuilder.DropForeignKey(
                name: "FK_Statements_Expressions_ValueExpressionId",
                table: "Statements");

            migrationBuilder.DropForeignKey(
                name: "FK_Statements_Expressions_WhileStatement_ConditionExpressionId",
                table: "Statements");

            migrationBuilder.DropIndex(
                name: "IX_Statements_ExpressionToEvaluateExpressionId",
                table: "Statements");

            migrationBuilder.DropIndex(
                name: "IX_Statements_ConditionExpressionId",
                table: "Statements");

            migrationBuilder.DropIndex(
                name: "IX_Statements_ValueExpressionId",
                table: "Statements");

            migrationBuilder.DropIndex(
                name: "IX_BlockStatement_WhileStatementStatementId",
                table: "BlockStatement");

            migrationBuilder.DropColumn(
                name: "ExpressionToEvaluateExpressionId",
                table: "Statements");

            migrationBuilder.DropColumn(
                name: "ConditionExpressionId",
                table: "Statements");

            migrationBuilder.DropColumn(
                name: "ValueExpressionId",
                table: "Statements");

            migrationBuilder.DropColumn(
                name: "VariableName",
                table: "Statements");

            migrationBuilder.DropColumn(
                name: "VariableDefinitionStatement_VariableName",
                table: "Statements");

            migrationBuilder.DropColumn(
                name: "VariableType",
                table: "Statements");

            migrationBuilder.DropColumn(
                name: "WhileStatementStatementId",
                table: "BlockStatement");

            migrationBuilder.RenameColumn(
                name: "WhileStatement_ConditionExpressionId",
                table: "Statements",
                newName: "ConditionExpressionId");

            migrationBuilder.RenameIndex(
                name: "IX_Statements_WhileStatement_ConditionExpressionId",
                table: "Statements",
                newName: "IX_Statements_ConditionExpressionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Statements_Expressions_ConditionExpressionId",
                table: "Statements",
                column: "ConditionExpressionId",
                principalTable: "Expressions",
                principalColumn: "ExpressionId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
