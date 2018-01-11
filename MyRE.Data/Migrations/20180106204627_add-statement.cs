using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MyRE.Data.Migrations
{
    public partial class addstatement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Statements",
                columns: table => new
                {
                    Event = table.Column<string>(nullable: true),
                    ConditionExpressionId = table.Column<long>(nullable: true),
                    StatementId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Discriminator = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statements", x => x.StatementId);
                    table.ForeignKey(
                        name: "FK_Statements_Expressions_ConditionExpressionId",
                        column: x => x.ConditionExpressionId,
                        principalTable: "Expressions",
                        principalColumn: "ExpressionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BlockStatement",
                columns: table => new
                {
                    BlockStatementId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EventHandlerStatementStatementId = table.Column<long>(nullable: true),
                    IfStatementStatementId = table.Column<long>(nullable: true),
                    Position = table.Column<int>(nullable: false),
                    StatementId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockStatement", x => x.BlockStatementId);
                    table.ForeignKey(
                        name: "FK_BlockStatement_Statements_EventHandlerStatementStatementId",
                        column: x => x.EventHandlerStatementStatementId,
                        principalTable: "Statements",
                        principalColumn: "StatementId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlockStatement_Statements_IfStatementStatementId",
                        column: x => x.IfStatementStatementId,
                        principalTable: "Statements",
                        principalColumn: "StatementId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlockStatement_Statements_StatementId",
                        column: x => x.StatementId,
                        principalTable: "Statements",
                        principalColumn: "StatementId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlockStatement_EventHandlerStatementStatementId",
                table: "BlockStatement",
                column: "EventHandlerStatementStatementId");

            migrationBuilder.CreateIndex(
                name: "IX_BlockStatement_IfStatementStatementId",
                table: "BlockStatement",
                column: "IfStatementStatementId");

            migrationBuilder.CreateIndex(
                name: "IX_BlockStatement_StatementId",
                table: "BlockStatement",
                column: "StatementId");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_ConditionExpressionId",
                table: "Statements",
                column: "ConditionExpressionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlockStatement");

            migrationBuilder.DropTable(
                name: "Statements");
        }
    }
}
