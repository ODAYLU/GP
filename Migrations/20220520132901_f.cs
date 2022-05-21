using Microsoft.EntityFrameworkCore.Migrations;

namespace GP.Migrations
{
    public partial class f : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "publish",
                table: "TEstates",
                type: "bit",
                nullable: false,
                defaultValueSql: "1",
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "publish",
                table: "TEstates",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValueSql: "1");
        }
    }
}
