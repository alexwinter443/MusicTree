using Microsoft.EntityFrameworkCore.Migrations;

namespace Milestone.Migrations
{
    public partial class InitialCreate7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "accountname",
                table: "AudioFiles");

            migrationBuilder.DropColumn(
                name: "resourcegroup",
                table: "AudioFiles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "accountname",
                table: "AudioFiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "resourcegroup",
                table: "AudioFiles",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
