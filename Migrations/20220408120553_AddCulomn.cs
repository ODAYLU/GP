using Microsoft.EntityFrameworkCore.Migrations;

namespace GP.Migrations
{
    public partial class AddCulomn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameRole",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameRole",
                table: "AspNetUsers");
        }
    }
}
