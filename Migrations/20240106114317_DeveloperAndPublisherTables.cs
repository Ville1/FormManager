using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FormManager.Migrations
{
    /// <inheritdoc />
    public partial class DeveloperAndPublisherTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeveloperName",
                table: "VideoGames");

            migrationBuilder.DropColumn(
                name: "PublisherName",
                table: "VideoGames");

            migrationBuilder.AddColumn<Guid>(
                name: "DeveloperId",
                table: "VideoGames",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PublisherId",
                table: "VideoGames",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Developers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Developers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Publishers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publishers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VideoGames_DeveloperId",
                table: "VideoGames",
                column: "DeveloperId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoGames_PublisherId",
                table: "VideoGames",
                column: "PublisherId");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoGames_Developers_DeveloperId",
                table: "VideoGames",
                column: "DeveloperId",
                principalTable: "Developers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoGames_Publishers_PublisherId",
                table: "VideoGames",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideoGames_Developers_DeveloperId",
                table: "VideoGames");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoGames_Publishers_PublisherId",
                table: "VideoGames");

            migrationBuilder.DropTable(
                name: "Developers");

            migrationBuilder.DropTable(
                name: "Publishers");

            migrationBuilder.DropIndex(
                name: "IX_VideoGames_DeveloperId",
                table: "VideoGames");

            migrationBuilder.DropIndex(
                name: "IX_VideoGames_PublisherId",
                table: "VideoGames");

            migrationBuilder.DropColumn(
                name: "DeveloperId",
                table: "VideoGames");

            migrationBuilder.DropColumn(
                name: "PublisherId",
                table: "VideoGames");

            migrationBuilder.AddColumn<string>(
                name: "DeveloperName",
                table: "VideoGames",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PublisherName",
                table: "VideoGames",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
