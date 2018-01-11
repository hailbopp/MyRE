using Microsoft.EntityFrameworkCore.Migrations;

namespace MyRE.Data.Migrations
{
    public partial class AddProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Region",
                table: "AppInstances",
                newName: "InstanceServerBaseUri");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "AppInstances",
                newName: "ExecutionToken");

            migrationBuilder.AddColumn<string>(
                name: "AccessToken",
                table: "AppInstances",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessToken",
                table: "AppInstances");

            migrationBuilder.RenameColumn(
                name: "InstanceServerBaseUri",
                table: "AppInstances",
                newName: "Region");

            migrationBuilder.RenameColumn(
                name: "ExecutionToken",
                table: "AppInstances",
                newName: "LocationId");
        }
    }
}
