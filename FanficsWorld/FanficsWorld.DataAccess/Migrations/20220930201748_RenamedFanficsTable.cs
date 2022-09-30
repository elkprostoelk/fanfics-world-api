using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FanficsWorld.DataAccess.Migrations
{
    public partial class RenamedFanficsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fanfic_AspNetUsers_AuthorId",
                table: "Fanfic");

            migrationBuilder.DropForeignKey(
                name: "FK_FanficCoauthors_Fanfic_FanficId",
                table: "FanficCoauthors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fanfic",
                table: "Fanfic");

            migrationBuilder.RenameTable(
                name: "Fanfic",
                newName: "Fanfics");

            migrationBuilder.RenameIndex(
                name: "IX_Fanfic_AuthorId",
                table: "Fanfics",
                newName: "IX_Fanfics_AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fanfics",
                table: "Fanfics",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FanficCoauthors_Fanfics_FanficId",
                table: "FanficCoauthors",
                column: "FanficId",
                principalTable: "Fanfics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fanfics_AspNetUsers_AuthorId",
                table: "Fanfics",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FanficCoauthors_Fanfics_FanficId",
                table: "FanficCoauthors");

            migrationBuilder.DropForeignKey(
                name: "FK_Fanfics_AspNetUsers_AuthorId",
                table: "Fanfics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fanfics",
                table: "Fanfics");

            migrationBuilder.RenameTable(
                name: "Fanfics",
                newName: "Fanfic");

            migrationBuilder.RenameIndex(
                name: "IX_Fanfics_AuthorId",
                table: "Fanfic",
                newName: "IX_Fanfic_AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fanfic",
                table: "Fanfic",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Fanfic_AspNetUsers_AuthorId",
                table: "Fanfic",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FanficCoauthors_Fanfic_FanficId",
                table: "FanficCoauthors",
                column: "FanficId",
                principalTable: "Fanfic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
