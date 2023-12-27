using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FormManager.Migrations
{
    /// <inheritdoc />
    public partial class MoreVideoGameData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeveloperName",
                table: "VideoGames",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Genre",
                table: "VideoGames",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
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

            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "VideoGames",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeveloperName",
                table: "VideoGames");

            migrationBuilder.DropColumn(
                name: "Genre",
                table: "VideoGames");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "VideoGames");

            migrationBuilder.DropColumn(
                name: "PublisherName",
                table: "VideoGames");

            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "VideoGames");
        }
    }
}
