using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roborally.infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class LobbyToContinueGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameLobbyRequiredUsers",
                columns: table => new
                {
                    GameLobby1GameId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequiredUsersUsername = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameLobbyRequiredUsers", x => new { x.GameLobby1GameId, x.RequiredUsersUsername });
                    table.ForeignKey(
                        name: "FK_GameLobbyRequiredUsers_GameLobby_GameLobby1GameId",
                        column: x => x.GameLobby1GameId,
                        principalTable: "GameLobby",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameLobbyRequiredUsers_Users_RequiredUsersUsername",
                        column: x => x.RequiredUsersUsername,
                        principalTable: "Users",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameLobbyRequiredUsers_RequiredUsersUsername",
                table: "GameLobbyRequiredUsers",
                column: "RequiredUsersUsername");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameLobbyRequiredUsers");
        }
    }
}
