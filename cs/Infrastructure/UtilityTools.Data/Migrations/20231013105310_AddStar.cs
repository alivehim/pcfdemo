using Microsoft.EntityFrameworkCore.Migrations;

namespace UtilityTools.Data.Migrations
{
    public partial class AddStar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Star",
                table: "MediaKeyword",
                type: "INTEGER",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Star",
                table: "MediaKeyword");
        }
    }
}
