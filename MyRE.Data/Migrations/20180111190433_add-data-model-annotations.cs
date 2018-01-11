using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MyRE.Data.Migrations
{
    public partial class adddatamodelannotations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VariableName",
                table: "Statements");

            migrationBuilder.DropColumn(
                name: "VariableDefinitionStatement_VariableName",
                table: "Statements");

            migrationBuilder.AlterColumn<int>(
                name: "VariableType",
                table: "Statements",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "Statements",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Event",
                table: "Statements",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "VariableNameExpressionExpressionId",
                table: "Statements",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "VariableDefinitionStatement_VariableNameExpressionExpressionId",
                table: "Statements",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Routines",
                maxLength: 1024,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VariableName",
                table: "Expressions",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FunctionName",
                table: "Expressions",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "Expressions",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_Statements_VariableNameExpressionExpressionId",
                table: "Statements",
                column: "VariableNameExpressionExpressionId");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_VariableDefinitionStatement_VariableNameExpressionExpressionId",
                table: "Statements",
                column: "VariableDefinitionStatement_VariableNameExpressionExpressionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Statements_Expressions_VariableNameExpressionExpressionId",
                table: "Statements",
                column: "VariableNameExpressionExpressionId",
                principalTable: "Expressions",
                principalColumn: "ExpressionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Statements_Expressions_VariableDefinitionStatement_VariableNameExpressionExpressionId",
                table: "Statements",
                column: "VariableDefinitionStatement_VariableNameExpressionExpressionId",
                principalTable: "Expressions",
                principalColumn: "ExpressionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Statements_Expressions_VariableNameExpressionExpressionId",
                table: "Statements");

            migrationBuilder.DropForeignKey(
                name: "FK_Statements_Expressions_VariableDefinitionStatement_VariableNameExpressionExpressionId",
                table: "Statements");

            migrationBuilder.DropIndex(
                name: "IX_Statements_VariableNameExpressionExpressionId",
                table: "Statements");

            migrationBuilder.DropIndex(
                name: "IX_Statements_VariableDefinitionStatement_VariableNameExpressionExpressionId",
                table: "Statements");

            migrationBuilder.DropColumn(
                name: "VariableNameExpressionExpressionId",
                table: "Statements");

            migrationBuilder.DropColumn(
                name: "VariableDefinitionStatement_VariableNameExpressionExpressionId",
                table: "Statements");

            migrationBuilder.AlterColumn<string>(
                name: "VariableType",
                table: "Statements",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "Statements",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "Event",
                table: "Statements",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VariableName",
                table: "Statements",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VariableDefinitionStatement_VariableName",
                table: "Statements",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Routines",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1024,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VariableName",
                table: "Expressions",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FunctionName",
                table: "Expressions",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "Expressions",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 32);
        }
    }
}
