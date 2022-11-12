using Microsoft.EntityFrameworkCore.Migrations;

namespace Milestone.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          /*  migrationBuilder.RenameColumn(
                name: "DashUrl",
                table: "AudioFiles",
                newName: "resourcegroup");

            migrationBuilder.AddColumn<int>(
                name: "FK_audioID",
                table: "AudioFiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "AudioFiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "accountname",
                table: "AudioFiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "filepath",
                table: "AudioFiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "outputassetname",
                table: "AudioFiles",
                type: "nvarchar(max)",
                nullable: true);*/
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FK_audioID",
                table: "AudioFiles");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "AudioFiles");

            migrationBuilder.DropColumn(
                name: "accountname",
                table: "AudioFiles");

            migrationBuilder.DropColumn(
                name: "filepath",
                table: "AudioFiles");

            migrationBuilder.DropColumn(
                name: "outputassetname",
                table: "AudioFiles");

            migrationBuilder.RenameColumn(
                name: "resourcegroup",
                table: "AudioFiles",
                newName: "DashUrl");
        }
    }
}
