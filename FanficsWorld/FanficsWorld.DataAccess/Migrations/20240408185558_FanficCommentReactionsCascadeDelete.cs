using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FanficsWorld.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FanficCommentReactionsCascadeDelete : Migration
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

            migrationBuilder.DropForeignKey(
                name: "FK_FanficCommentReactions_AspNetUsers_UserId",
                table: "FanficCommentReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_FanficComments_AspNetUsers_AuthorId",
                table: "FanficComments");

            migrationBuilder.DropForeignKey(
                name: "FK_FanficComments_Fanfics_FanficId",
                table: "FanficComments");

            migrationBuilder.DropForeignKey(
                name: "FK_FanficFandoms_Fandoms_FandomId",
                table: "FanficFandoms");

            migrationBuilder.DropForeignKey(
                name: "FK_FanficFandoms_Fanfics_FanficId",
                table: "FanficFandoms");

            migrationBuilder.DropForeignKey(
                name: "FK_FanficTags_Fanfics_FanficId",
                table: "FanficTags");

            migrationBuilder.DropForeignKey(
                name: "FK_FanficTags_Tags_TagId",
                table: "FanficTags");

            migrationBuilder.AddForeignKey(
                name: "FK_FanficCoauthors_AspNetUsers_CoauthorId",
                table: "FanficCoauthors",
                column: "CoauthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FanficCoauthors_Fanfics_FanficId",
                table: "FanficCoauthors",
                column: "FanficId",
                principalTable: "Fanfics",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FanficCommentReactions_AspNetUsers_UserId",
                table: "FanficCommentReactions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FanficComments_AspNetUsers_AuthorId",
                table: "FanficComments",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FanficComments_Fanfics_FanficId",
                table: "FanficComments",
                column: "FanficId",
                principalTable: "Fanfics",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FanficFandoms_Fandoms_FandomId",
                table: "FanficFandoms",
                column: "FandomId",
                principalTable: "Fandoms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FanficFandoms_Fanfics_FanficId",
                table: "FanficFandoms",
                column: "FanficId",
                principalTable: "Fanfics",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FanficTags_Fanfics_FanficId",
                table: "FanficTags",
                column: "FanficId",
                principalTable: "Fanfics",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FanficTags_Tags_TagId",
                table: "FanficTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id");
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

            migrationBuilder.DropForeignKey(
                name: "FK_FanficCommentReactions_AspNetUsers_UserId",
                table: "FanficCommentReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_FanficComments_AspNetUsers_AuthorId",
                table: "FanficComments");

            migrationBuilder.DropForeignKey(
                name: "FK_FanficComments_Fanfics_FanficId",
                table: "FanficComments");

            migrationBuilder.DropForeignKey(
                name: "FK_FanficFandoms_Fandoms_FandomId",
                table: "FanficFandoms");

            migrationBuilder.DropForeignKey(
                name: "FK_FanficFandoms_Fanfics_FanficId",
                table: "FanficFandoms");

            migrationBuilder.DropForeignKey(
                name: "FK_FanficTags_Fanfics_FanficId",
                table: "FanficTags");

            migrationBuilder.DropForeignKey(
                name: "FK_FanficTags_Tags_TagId",
                table: "FanficTags");

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

            migrationBuilder.AddForeignKey(
                name: "FK_FanficCommentReactions_AspNetUsers_UserId",
                table: "FanficCommentReactions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FanficComments_AspNetUsers_AuthorId",
                table: "FanficComments",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FanficComments_Fanfics_FanficId",
                table: "FanficComments",
                column: "FanficId",
                principalTable: "Fanfics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FanficFandoms_Fandoms_FandomId",
                table: "FanficFandoms",
                column: "FandomId",
                principalTable: "Fandoms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FanficFandoms_Fanfics_FanficId",
                table: "FanficFandoms",
                column: "FanficId",
                principalTable: "Fanfics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FanficTags_Fanfics_FanficId",
                table: "FanficTags",
                column: "FanficId",
                principalTable: "Fanfics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FanficTags_Tags_TagId",
                table: "FanficTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
