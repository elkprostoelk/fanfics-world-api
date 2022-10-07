using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FanficsWorld.DataAccess.Migrations
{
    public partial class AddedFandoms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fandoms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fandoms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FanficFandoms",
                columns: table => new
                {
                    FanficId = table.Column<long>(type: "bigint", nullable: false),
                    FandomId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FanficFandoms", x => new { x.FandomId, x.FanficId });
                    table.ForeignKey(
                        name: "FK_FanficFandoms_Fandoms_FandomId",
                        column: x => x.FandomId,
                        principalTable: "Fandoms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FanficFandoms_Fanfics_FanficId",
                        column: x => x.FanficId,
                        principalTable: "Fanfics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FanficFandoms_FanficId",
                table: "FanficFandoms",
                column: "FanficId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FanficFandoms");

            migrationBuilder.DropTable(
                name: "Fandoms");
        }
    }
}
