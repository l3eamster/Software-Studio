using Microsoft.EntityFrameworkCore.Migrations;

namespace DonutzStudio.Migrations
{
    public partial class UpdateUserModelAgain2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "created",
                table: "User",
                newName: "Created");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Created",
                table: "User",
                newName: "created");
        }
    }
}
