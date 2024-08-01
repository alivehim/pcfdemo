using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UtilityTools.Data.Migrations
{
    public partial class addpathanddate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "MediaHistory",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "MediaHistory",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "MediaHistory");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "MediaHistory");
        }
    }
}
