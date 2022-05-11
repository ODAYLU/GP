using Microsoft.EntityFrameworkCore.Migrations;

namespace GP.Migrations
{
    public partial class f6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "IDEstet",
                table: "TContract",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_TContract_IDEstet",
                table: "TContract",
                column: "IDEstet");

            migrationBuilder.AddForeignKey(
                name: "FK_TContract_TEstates_IDEstet",
                table: "TContract",
                column: "IDEstet",
                principalTable: "TEstates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TContract_TEstates_IDEstet",
                table: "TContract");

            migrationBuilder.DropIndex(
                name: "IX_TContract_IDEstet",
                table: "TContract");

            migrationBuilder.DropColumn(
                name: "IDEstet",
                table: "TContract");
        }
    }
}
