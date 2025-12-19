using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roborally.infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class gameLobbyManyToMany : Migration
    {
        /// <inheritdoc />
/// <author name="Sachin Baral 2025-09-19 13:01:07 +0200 12" />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameLobby_Users_HostUsername",
                table: "GameLobby");

            migrationBuilder.DropIndex(
                name: "IX_GameLobby_HostUsername",
                table: "GameLobby");

            migrationBuilder.CreateTable(
                name: "GameLobbyUsers",
                columns: table => new
                {
                    GameLobbyGameId = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameLobbyUsers", x => new { x.GameLobbyGameId, x.Username });
                    table.ForeignKey(
                        name: "FK_GameLobbyUsers_GameLobby_GameLobbyGameId",
                        column: x => x.GameLobbyGameId,
                        principalTable: "GameLobby",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameLobbyUsers_Users_Username",
                        column: x => x.Username,
                        principalTable: "Users",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameLobbyUsers_Username",
                table: "GameLobbyUsers",
                column: "Username");
        }

        /// <inheritdoc />
/// <author name="Sachin Baral 2025-09-19 13:01:07 +0200 53" />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameLobbyUsers");

            migrationBuilder.CreateIndex(
                name: "IX_GameLobby_HostUsername",
                table: "GameLobby",
                column: "HostUsername",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GameLobby_Users_HostUsername",
                table: "GameLobby",
                column: "HostUsername",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);
        }
    }
}