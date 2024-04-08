using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FanficsWorld.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedFanficCommentsWithReactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FanficComments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FanficId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FanficComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FanficComments_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FanficComments_Fanfics_FanficId",
                        column: x => x.FanficId,
                        principalTable: "Fanfics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FanficCommentReactions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsLike = table.Column<bool>(type: "bit", nullable: false),
                    FanficCommentId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FanficCommentReactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FanficCommentReactions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FanficCommentReactions_FanficComments_FanficCommentId",
                        column: x => x.FanficCommentId,
                        principalTable: "FanficComments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FanficCommentReactions_FanficCommentId",
                table: "FanficCommentReactions",
                column: "FanficCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_FanficCommentReactions_UserId",
                table: "FanficCommentReactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FanficComments_AuthorId",
                table: "FanficComments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_FanficComments_FanficId",
                table: "FanficComments",
                column: "FanficId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FanficCommentReactions");

            migrationBuilder.DropTable(
                name: "FanficComments");
        }
    }
}
