using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyRE.Data.Migrations
{
    public partial class Addblocktype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlockStatement_Statements_EventHandlerStatementStatementId",
                table: "BlockStatement");

            migrationBuilder.DropForeignKey(
                name: "FK_BlockStatement_Statements_IfStatementStatementId",
                table: "BlockStatement");

            migrationBuilder.DropForeignKey(
                name: "FK_BlockStatement_Statements_WhileStatementStatementId",
                table: "BlockStatement");

            migrationBuilder.DropIndex(
                name: "IX_BlockStatement_EventHandlerStatementStatementId",
                table: "BlockStatement");

            migrationBuilder.DropIndex(
                name: "IX_BlockStatement_IfStatementStatementId",
                table: "BlockStatement");

            migrationBuilder.DropColumn(
                name: "EventHandlerStatementStatementId",
                table: "BlockStatement");

            migrationBuilder.DropColumn(
                name: "IfStatementStatementId",
                table: "BlockStatement");

            migrationBuilder.RenameColumn(
                name: "WhileStatementStatementId",
                table: "BlockStatement",
                newName: "BlockId");

            migrationBuilder.RenameIndex(
                name: "IX_BlockStatement_WhileStatementStatementId",
                table: "BlockStatement",
                newName: "IX_BlockStatement_BlockId");

            migrationBuilder.AddColumn<long>(
                name: "BlockId",
                table: "Statements",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "IfStatement_BlockId",
                table: "Statements",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WhileStatement_BlockId",
                table: "Statements",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Block",
                columns: table => new
                {
                    BlockId = table.Column<long>()
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Block", x => x.BlockId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Statements_BlockId",
                table: "Statements",
                column: "BlockId");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_IfStatement_BlockId",
                table: "Statements",
                column: "IfStatement_BlockId");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_WhileStatement_BlockId",
                table: "Statements",
                column: "WhileStatement_BlockId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlockStatement_Block_BlockId",
                table: "BlockStatement",
                column: "BlockId",
                principalTable: "Block",
                principalColumn: "BlockId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Statements_Block_BlockId",
                table: "Statements",
                column: "BlockId",
                principalTable: "Block",
                principalColumn: "BlockId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Statements_Block_IfStatement_BlockId",
                table: "Statements",
                column: "IfStatement_BlockId",
                principalTable: "Block",
                principalColumn: "BlockId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Statements_Block_WhileStatement_BlockId",
                table: "Statements",
                column: "WhileStatement_BlockId",
                principalTable: "Block",
                principalColumn: "BlockId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlockStatement_Block_BlockId",
                table: "BlockStatement");

            migrationBuilder.DropForeignKey(
                name: "FK_Statements_Block_BlockId",
                table: "Statements");

            migrationBuilder.DropForeignKey(
                name: "FK_Statements_Block_IfStatement_BlockId",
                table: "Statements");

            migrationBuilder.DropForeignKey(
                name: "FK_Statements_Block_WhileStatement_BlockId",
                table: "Statements");

            migrationBuilder.DropTable(
                name: "Block");

            migrationBuilder.DropIndex(
                name: "IX_Statements_BlockId",
                table: "Statements");

            migrationBuilder.DropIndex(
                name: "IX_Statements_IfStatement_BlockId",
                table: "Statements");

            migrationBuilder.DropIndex(
                name: "IX_Statements_WhileStatement_BlockId",
                table: "Statements");

            migrationBuilder.DropColumn(
                name: "BlockId",
                table: "Statements");

            migrationBuilder.DropColumn(
                name: "IfStatement_BlockId",
                table: "Statements");

            migrationBuilder.DropColumn(
                name: "WhileStatement_BlockId",
                table: "Statements");

            migrationBuilder.RenameColumn(
                name: "BlockId",
                table: "BlockStatement",
                newName: "WhileStatementStatementId");

            migrationBuilder.RenameIndex(
                name: "IX_BlockStatement_BlockId",
                table: "BlockStatement",
                newName: "IX_BlockStatement_WhileStatementStatementId");

            migrationBuilder.AddColumn<long>(
                name: "EventHandlerStatementStatementId",
                table: "BlockStatement",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "IfStatementStatementId",
                table: "BlockStatement",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlockStatement_EventHandlerStatementStatementId",
                table: "BlockStatement",
                column: "EventHandlerStatementStatementId");

            migrationBuilder.CreateIndex(
                name: "IX_BlockStatement_IfStatementStatementId",
                table: "BlockStatement",
                column: "IfStatementStatementId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlockStatement_Statements_EventHandlerStatementStatementId",
                table: "BlockStatement",
                column: "EventHandlerStatementStatementId",
                principalTable: "Statements",
                principalColumn: "StatementId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BlockStatement_Statements_IfStatementStatementId",
                table: "BlockStatement",
                column: "IfStatementStatementId",
                principalTable: "Statements",
                principalColumn: "StatementId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BlockStatement_Statements_WhileStatementStatementId",
                table: "BlockStatement",
                column: "WhileStatementStatementId",
                principalTable: "Statements",
                principalColumn: "StatementId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
