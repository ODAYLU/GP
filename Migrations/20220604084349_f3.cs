using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GP.Migrations
{
    public partial class f3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OnDate",
                table: "TComments",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "Getdate()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnDate",
                table: "TComments");
        }
    }
}
