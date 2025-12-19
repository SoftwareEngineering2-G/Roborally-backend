using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roborally.infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class playerEventsCorrectly : Migration
    {
        /// <inheritdoc />
/// <author name="Sachin Baral 2025-09-28 13:55:14 +0200 12" />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerEvents_Players_PlayerGameId_PlayerUsername",
                table: "PlayerEvents");

            migrationBuilder.DropIndex(
                name: "IX_PlayerEvents_PlayerGameId_PlayerUsername",
                table: "PlayerEvents");

            migrationBuilder.DropColumn(
                name: "PlayerGameId",
                table: "PlayerEvents");

            migrationBuilder.DropColumn(
                name: "PlayerUsername",
                table: "PlayerEvents");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerEvents_GameId_Username",
                table: "PlayerEvents",
                columns: new[] { "GameId", "Username" });

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerEvents_Players_GameId_Username",
                table: "PlayerEvents",
                columns: new[] { "GameId", "Username" },
                principalTable: "Players",
                principalColumns: new[] { "GameId", "Username" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
/// <author name="Sachin Baral 2025-09-28 13:55:14 +0200 45" />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerEvents_Players_GameId_Username",
                table: "PlayerEvents");

            migrationBuilder.DropIndex(
                name: "IX_PlayerEvents_GameId_Username",
                table: "PlayerEvents");

            migrationBuilder.AddColumn<Guid>(
                name: "PlayerGameId",
                table: "PlayerEvents",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlayerUsername",
                table: "PlayerEvents",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerEvents_PlayerGameId_PlayerUsername",
                table: "PlayerEvents",
                columns: new[] { "PlayerGameId", "PlayerUsername" });

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerEvents_Players_PlayerGameId_PlayerUsername",
                table: "PlayerEvents",
                columns: new[] { "PlayerGameId", "PlayerUsername" },
                principalTable: "Players",
                principalColumns: new[] { "GameId", "Username" });
        }
    }
}