using Microsoft.EntityFrameworkCore.Migrations;

namespace GP.Migrations
{
    public partial class f8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "isDone",
                table: "TContract",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDone",
                table: "TContract");
        }
    }
}
