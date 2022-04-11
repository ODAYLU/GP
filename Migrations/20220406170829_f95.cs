using Microsoft.EntityFrameworkCore.Migrations;

namespace GP.Migrations
{
    public partial class f95 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TServices_TEstates_EstateId",
                table: "TServices");

            migrationBuilder.DropIndex(
                name: "IX_TServices_EstateId",
                table: "TServices");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "TServices");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EstateId",
                table: "TServices",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TServices_EstateId",
                table: "TServices",
                column: "EstateId");

            migrationBuilder.AddForeignKey(
                name: "FK_TServices_TEstates_EstateId",
                table: "TServices",
                column: "EstateId",
                principalTable: "TEstates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
