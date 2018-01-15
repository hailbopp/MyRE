using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MyRE.Data.Migrations
{
    public partial class Basemodelswithguidpks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    SecurityStamp = table.Column<string>(nullable: true),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

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
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountId = table.Column<Guid>(nullable: false),
                    RemoteAccountId = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountId);
                    table.UniqueConstraint("UNQ_RemoteAccountId", x => x.RemoteAccountId);
                    table.ForeignKey(
                        name: "FK_Accounts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
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
                    ExpressionToEvaluateExpressionId = table.Column<Guid>(nullable: true),
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
                        name: "FK_Statements_Expressions_ExpressionToEvaluateExpressionId",
                        column: x => x.ExpressionToEvaluateExpressionId,
                        principalTable: "Expressions",
                        principalColumn: "ExpressionId",
                        onDelete: ReferentialAction.Restrict);
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
                name: "AppInstances",
                columns: table => new
                {
                    AppInstanceId = table.Column<Guid>(nullable: false),
                    AccessToken = table.Column<string>(nullable: true),
                    AccountId = table.Column<Guid>(nullable: false),
                    InstanceServerBaseUri = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    RemoteAppId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppInstances", x => x.AppInstanceId);
                    table.UniqueConstraint("UNQ_RemoteAppId", x => x.RemoteAppId);
                    table.ForeignKey(
                        name: "FK_AppInstances_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ParentInstanceId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectId);
                    table.ForeignKey(
                        name: "FK_Projects_AppInstances_ParentInstanceId",
                        column: x => x.ParentInstanceId,
                        principalTable: "AppInstances",
                        principalColumn: "AppInstanceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Routines",
                columns: table => new
                {
                    RoutineId = table.Column<Guid>(nullable: false),
                    BlockId = table.Column<Guid>(nullable: true),
                    Description = table.Column<string>(maxLength: 4096, nullable: true),
                    ExecutionMethod = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 1024, nullable: true),
                    ProjectId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routines", x => x.RoutineId);
                    table.ForeignKey(
                        name: "FK_Routines_Block_BlockId",
                        column: x => x.BlockId,
                        principalTable: "Block",
                        principalColumn: "BlockId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Routines_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserId",
                table: "Accounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppInstances_AccountId",
                table: "AppInstances",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

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
                name: "IX_Projects_ParentInstanceId",
                table: "Projects",
                column: "ParentInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Routines_BlockId",
                table: "Routines",
                column: "BlockId");

            migrationBuilder.CreateIndex(
                name: "IX_Routines_ProjectId",
                table: "Routines",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_ExpressionToEvaluateExpressionId",
                table: "Statements",
                column: "ExpressionToEvaluateExpressionId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BlockStatement");

            migrationBuilder.DropTable(
                name: "FunctionParameter");

            migrationBuilder.DropTable(
                name: "Routines");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Statements");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Expressions");

            migrationBuilder.DropTable(
                name: "Block");

            migrationBuilder.DropTable(
                name: "AppInstances");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
