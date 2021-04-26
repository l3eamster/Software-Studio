using Microsoft.EntityFrameworkCore.Migrations;

namespace DonutzStudio.Migrations
{
    public partial class UpdateLabModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Lab",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemImage",
                table: "Lab",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Lab");

            migrationBuilder.DropColumn(
                name: "ItemImage",
                table: "Lab");
        }
    }
}
