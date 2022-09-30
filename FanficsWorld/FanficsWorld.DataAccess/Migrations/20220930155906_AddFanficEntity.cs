using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FanficsWorld.DataAccess.Migrations
{
    public partial class AddFanficEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fanfic",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Annotation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AuthorId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fanfic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fanfic_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FanficCoauthors",
                columns: table => new
                {
                    FanficId = table.Column<long>(type: "bigint", nullable: false),
                    CoauthorId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FanficCoauthors", x => new { x.FanficId, x.CoauthorId });
                    table.ForeignKey(
                        name: "FK_FanficCoauthors_AspNetUsers_CoauthorId",
                        column: x => x.CoauthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FanficCoauthors_Fanfic_FanficId",
                        column: x => x.FanficId,
                        principalTable: "Fanfic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fanfic_AuthorId",
                table: "Fanfic",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_FanficCoauthors_CoauthorId",
                table: "FanficCoauthors",
                column: "CoauthorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FanficCoauthors");

            migrationBuilder.DropTable(
                name: "Fanfic");
        }
    }
}
