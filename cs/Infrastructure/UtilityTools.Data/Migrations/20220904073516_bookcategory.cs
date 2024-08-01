using Microsoft.EntityFrameworkCore.Migrations;

namespace UtilityTools.Data.Migrations
{
    public partial class bookcategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PngCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PngCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PngImage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Location = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PngImage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PngImageCategoryRelation",
                columns: table => new
                {
                    PngImageId = table.Column<int>(type: "INTEGER", nullable: false),
                    LabelId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PngImageCategoryRelation", x => new { x.PngImageId, x.LabelId });
                    table.ForeignKey(
                        name: "FK_PngImageCategoryRelation_PngCategory_LabelId",
                        column: x => x.LabelId,
                        principalTable: "PngCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PngImageCategoryRelation_PngImage_PngImageId",
                        column: x => x.PngImageId,
                        principalTable: "PngImage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PngImageCategoryRelation_LabelId",
                table: "PngImageCategoryRelation",
                column: "LabelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PngImageCategoryRelation");

            migrationBuilder.DropTable(
                name: "PngCategory");

            migrationBuilder.DropTable(
                name: "PngImage");
        }
    }
}
