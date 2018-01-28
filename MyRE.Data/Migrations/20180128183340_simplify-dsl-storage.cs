using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MyRE.Data.Migrations
{
    public partial class simplifydslstorage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlockStatement");

            migrationBuilder.DropTable(
                name: "FunctionParameter");

            migrationBuilder.DropTable(
                name: "Routines");

            migrationBuilder.DropTable(
                name: "Statements");

            migrationBuilder.DropTable(
                name: "Expressions");

            migrationBuilder.DropTable(
                name: "Block");

            migrationBuilder.CreateTable(
                name: "ProjectSourceVersions",
                columns: table => new
                {
                    ProjectSourceId = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "getutcdate()"),
                    ExpressionTree = table.Column<string>(nullable: true),
                    ProjectId = table.Column<Guid>(nullable: false),
                    Source = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectSourceVersions", x => x.ProjectSourceId);
                    table.ForeignKey(
                        name: "FK_ProjectSourceVersions_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSourceVersions_ProjectId",
                table: "ProjectSourceVersions",
                column: "ProjectId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectSourceVersions");

            migrationBuilder.CreateTable(
                name: "Block",
                columns: table => new
                {
                    BlockId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Block", x => x.BlockId);
                });

            migrationBuilder.CreateTable(
                name: "Expressions",
                columns: table => new
                {
                    ExpressionId = table.Column<Guid>(nullable: false),
                    Discriminator = table.Column<string>(maxLength: 32, nullable: false),
                    FunctionName = table.Column<string>(maxLength: 64, nullable: true),
                    Value = table.Column<string>(nullable: true),
                    ValueType = table.Column<int>(nullable: true),
                    VariableName = table.Column<string>(maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expressions", x => x.ExpressionId);
                });

            migrationBuilder.CreateTable(
                name: "Routines",
                columns: table => new
                {
                    RoutineId = table.Column<Guid>(nullable: false),
                    BlockId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(maxLength: 4096, nullable: true),
                    ExecutionMethod = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 1024, nullable: true),
                    ProjectId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routines", x => x.RoutineId);
                    table.ForeignKey(
                        name: "FK_Routines_Block_BlockId",
                        column: x => x.BlockId,
                        principalTable: "Block",
                        principalColumn: "BlockId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Routines_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FunctionParameter",
                columns: table => new
                {
                    FunctionParameterId = table.Column<Guid>(nullable: false),
                    InvocationExpressionExpressionId = table.Column<Guid>(nullable: true),
                    Position = table.Column<int>(nullable: false),
                    ValueExpressionId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FunctionParameter", x => x.FunctionParameterId);
                    table.ForeignKey(
                        name: "FK_FunctionParameter_Expressions_InvocationExpressionExpressionId",
                        column: x => x.InvocationExpressionExpressionId,
                        principalTable: "Expressions",
                        principalColumn: "ExpressionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FunctionParameter_Expressions_ValueExpressionId",
                        column: x => x.ValueExpressionId,
                        principalTable: "Expressions",
                        principalColumn: "ExpressionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Statements",
                columns: table => new
                {
                    ExpressionToEvaluateId = table.Column<Guid>(nullable: true),
                    BlockId = table.Column<Guid>(nullable: true),
                    Event = table.Column<string>(maxLength: 128, nullable: true),
                    IfStatement_BlockId = table.Column<Guid>(nullable: true),
                    ConditionExpressionId = table.Column<Guid>(nullable: true),
                    StatementId = table.Column<Guid>(nullable: false),
                    Discriminator = table.Column<string>(maxLength: 32, nullable: false),
                    ValueExpressionId = table.Column<Guid>(nullable: true),
                    VariableNameExpressionExpressionId = table.Column<Guid>(nullable: true),
                    VariableDefinitionStatement_VariableNameExpressionExpressionId = table.Column<Guid>(nullable: true),
                    VariableType = table.Column<int>(nullable: true),
                    WhileStatement_BlockId = table.Column<Guid>(nullable: true),
                    WhileStatement_ConditionExpressionId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statements", x => x.StatementId);
                    table.ForeignKey(
                        name: "FK_Statements_Expressions_ExpressionToEvaluateId",
                        column: x => x.ExpressionToEvaluateId,
                        principalTable: "Expressions",
                        principalColumn: "ExpressionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Statements_Block_BlockId",
                        column: x => x.BlockId,
                        principalTable: "Block",
                        principalColumn: "BlockId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Statements_Block_IfStatement_BlockId",
                        column: x => x.IfStatement_BlockId,
                        principalTable: "Block",
                        principalColumn: "BlockId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Statements_Expressions_ConditionExpressionId",
                        column: x => x.ConditionExpressionId,
                        principalTable: "Expressions",
                        principalColumn: "ExpressionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Statements_Expressions_ValueExpressionId",
                        column: x => x.ValueExpressionId,
                        principalTable: "Expressions",
                        principalColumn: "ExpressionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Statements_Expressions_VariableNameExpressionExpressionId",
                        column: x => x.VariableNameExpressionExpressionId,
                        principalTable: "Expressions",
                        principalColumn: "ExpressionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Statements_Expressions_VariableDefinitionStatement_VariableNameExpressionExpressionId",
                        column: x => x.VariableDefinitionStatement_VariableNameExpressionExpressionId,
                        principalTable: "Expressions",
                        principalColumn: "ExpressionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Statements_Block_WhileStatement_BlockId",
                        column: x => x.WhileStatement_BlockId,
                        principalTable: "Block",
                        principalColumn: "BlockId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Statements_Expressions_WhileStatement_ConditionExpressionId",
                        column: x => x.WhileStatement_ConditionExpressionId,
                        principalTable: "Expressions",
                        principalColumn: "ExpressionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BlockStatement",
                columns: table => new
                {
                    BlockStatementId = table.Column<Guid>(nullable: false),
                    BlockId = table.Column<Guid>(nullable: true),
                    Position = table.Column<int>(nullable: false),
                    StatementId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockStatement", x => x.BlockStatementId);
                    table.ForeignKey(
                        name: "FK_BlockStatement_Block_BlockId",
                        column: x => x.BlockId,
                        principalTable: "Block",
                        principalColumn: "BlockId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlockStatement_Statements_StatementId",
                        column: x => x.StatementId,
                        principalTable: "Statements",
                        principalColumn: "StatementId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlockStatement_BlockId",
                table: "BlockStatement",
                column: "BlockId");

            migrationBuilder.CreateIndex(
                name: "IX_BlockStatement_StatementId",
                table: "BlockStatement",
                column: "StatementId");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionParameter_InvocationExpressionExpressionId",
                table: "FunctionParameter",
                column: "InvocationExpressionExpressionId");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionParameter_ValueExpressionId",
                table: "FunctionParameter",
                column: "ValueExpressionId");

            migrationBuilder.CreateIndex(
                name: "IX_Routines_BlockId",
                table: "Routines",
                column: "BlockId");

            migrationBuilder.CreateIndex(
                name: "IX_Routines_ProjectId",
                table: "Routines",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_ExpressionToEvaluateId",
                table: "Statements",
                column: "ExpressionToEvaluateId");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_BlockId",
                table: "Statements",
                column: "BlockId");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_IfStatement_BlockId",
                table: "Statements",
                column: "IfStatement_BlockId");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_ConditionExpressionId",
                table: "Statements",
                column: "ConditionExpressionId");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_ValueExpressionId",
                table: "Statements",
                column: "ValueExpressionId");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_VariableNameExpressionExpressionId",
                table: "Statements",
                column: "VariableNameExpressionExpressionId");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_VariableDefinitionStatement_VariableNameExpressionExpressionId",
                table: "Statements",
                column: "VariableDefinitionStatement_VariableNameExpressionExpressionId");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_WhileStatement_BlockId",
                table: "Statements",
                column: "WhileStatement_BlockId");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_WhileStatement_ConditionExpressionId",
                table: "Statements",
                column: "WhileStatement_ConditionExpressionId");
        }
    }
}
