using Microsoft.EntityFrameworkCore.Migrations;

namespace DonutzStudio.Migrations
{
    public partial class UpdateLabModelAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LabImage",
                table: "Lab",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LabImage",
                table: "Lab");
        }
    }
}
