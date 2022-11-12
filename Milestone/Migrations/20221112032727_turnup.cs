using Microsoft.EntityFrameworkCore.Migrations;

namespace Milestone.Migrations
{
    public partial class turnup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "likes",
                table: "AudioFiles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "likes",
                table: "AudioFiles");
        }
    }
}
