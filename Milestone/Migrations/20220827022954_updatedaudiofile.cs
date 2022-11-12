using Microsoft.EntityFrameworkCore.Migrations;

namespace Milestone.Migrations
{
    public partial class updatedaudiofile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           /* migrationBuilder.CreateTable(
                name: "AudioFiles",
                columns: table => new
                {
                    AudioFileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    Genre = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    Key = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    BPM = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    DashUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AudioFiles", x => x.AudioFileId);
                });*/
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AudioFiles");
        }
    }
}
