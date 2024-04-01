using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FanficsWorld.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FanficFandomCascadeDeleting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FanficCoauthors_AspNetUsers_CoauthorId",
                table: "FanficCoauthors");

            migrationBuilder.DropForeignKey(
                name: "FK_FanficCoauthors_Fanfics_FanficId",
                table: "FanficCoauthors");

            migrationBuilder.AddForeignKey(
                name: "FK_FanficCoauthors_AspNetUsers_CoauthorId",
                table: "FanficCoauthors",
                column: "CoauthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FanficCoauthors_Fanfics_FanficId",
                table: "FanficCoauthors",
                column: "FanficId",
                principalTable: "Fanfics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FanficCoauthors_AspNetUsers_CoauthorId",
                table: "FanficCoauthors");

            migrationBuilder.DropForeignKey(
                name: "FK_FanficCoauthors_Fanfics_FanficId",
                table: "FanficCoauthors");

            migrationBuilder.AddForeignKey(
                name: "FK_FanficCoauthors_AspNetUsers_CoauthorId",
                table: "FanficCoauthors",
                column: "CoauthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FanficCoauthors_Fanfics_FanficId",
                table: "FanficCoauthors",
                column: "FanficId",
                principalTable: "Fanfics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
