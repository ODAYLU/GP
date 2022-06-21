using Microsoft.EntityFrameworkCore.Migrations;

namespace GP.Migrations
{
    public partial class f8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdUser",
                table: "TOpinion");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdUser",
                table: "TOpinion",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
