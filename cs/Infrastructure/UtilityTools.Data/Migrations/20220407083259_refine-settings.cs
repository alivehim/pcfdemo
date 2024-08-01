using Microsoft.EntityFrameworkCore.Migrations;

namespace UtilityTools.Data.Migrations
{
    public partial class refinesettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "D365AccessToken",
                table: "UtilityToolsSetting");

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
                name: "ShowMediaGetModule",
                table: "UtilityToolsSetting");

            migrationBuilder.RenameColumn(
                name: "SokankanUrl",
                table: "UtilityToolsSetting",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "M3u8LocationRoot",
                table: "UtilityToolsSetting",
                newName: "Key");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "UtilityToolsSetting",
                newName: "SokankanUrl");

            migrationBuilder.RenameColumn(
                name: "Key",
                table: "UtilityToolsSetting",
                newName: "M3u8LocationRoot");

            migrationBuilder.AddColumn<string>(
                name: "D365AccessToken",
                table: "UtilityToolsSetting",
                type: "TEXT",
                nullable: true);

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

            migrationBuilder.AddColumn<bool>(
                name: "ShowMediaGetModule",
                table: "UtilityToolsSetting",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
