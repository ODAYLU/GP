using Microsoft.EntityFrameworkCore.Migrations;

namespace GP.Migrations
{
    public partial class nw : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EstateId",
                table: "TComments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "TComments",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_TComments_EstateId",
                table: "TComments",
                column: "EstateId");

            migrationBuilder.AddForeignKey(
                name: "FK_TComments_TEstates_EstateId",
                table: "TComments",
                column: "EstateId",
                principalTable: "TEstates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TComments_TEstates_EstateId",
                table: "TComments");

            migrationBuilder.DropIndex(
                name: "IX_TComments_EstateId",
                table: "TComments");

            migrationBuilder.DropColumn(
                name: "EstateId",
                table: "TComments");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "TComments");
        }
    }
}
