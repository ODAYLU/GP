using Microsoft.EntityFrameworkCore.Migrations;

namespace GP.Migrations
{
    public partial class f5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TlikedEstates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUser = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdEstate = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TlikedEstates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TlikedEstates_AspNetUsers_IdUser",
                        column: x => x.IdUser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TlikedEstates_TEstates_IdEstate",
                        column: x => x.IdEstate,
                        principalTable: "TEstates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TlikedEstates_IdEstate",
                table: "TlikedEstates",
                column: "IdEstate");

            migrationBuilder.CreateIndex(
                name: "IX_TlikedEstates_IdUser",
                table: "TlikedEstates",
                column: "IdUser");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TlikedEstates");
        }
    }
}
