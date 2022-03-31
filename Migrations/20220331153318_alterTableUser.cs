using Microsoft.EntityFrameworkCore.Migrations;

namespace GP.Migrations
{
    public partial class alterTableUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role_User",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role_User",
                table: "AspNetUsers");
        }
    }
}
