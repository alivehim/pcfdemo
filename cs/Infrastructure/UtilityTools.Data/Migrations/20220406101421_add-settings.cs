using Microsoft.EntityFrameworkCore.Migrations;

namespace UtilityTools.Data.Migrations
{
    public partial class addsettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DisplayNotification",
                table: "UtilityToolsSetting",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DownloadRPCAddress",
                table: "UtilityToolsSetting",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FFMPEGLocation",
                table: "UtilityToolsSetting",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "M3u8LocationRoot",
                table: "UtilityToolsSetting",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowMediaGetModule",
                table: "UtilityToolsSetting",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SokankanUrl",
                table: "UtilityToolsSetting",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayNotification",
                table: "UtilityToolsSetting");

            migrationBuilder.DropColumn(
                name: "DownloadRPCAddress",
                table: "UtilityToolsSetting");

            migrationBuilder.DropColumn(
                name: "FFMPEGLocation",
                table: "UtilityToolsSetting");

            migrationBuilder.DropColumn(
                name: "M3u8LocationRoot",
                table: "UtilityToolsSetting");

            migrationBuilder.DropColumn(
                name: "ShowMediaGetModule",
                table: "UtilityToolsSetting");

            migrationBuilder.DropColumn(
                name: "SokankanUrl",
                table: "UtilityToolsSetting");
        }
    }
}
