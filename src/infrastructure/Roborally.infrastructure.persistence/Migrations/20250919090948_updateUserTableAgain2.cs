using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roborally.infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class updateUserTableAgain2 : Migration
    {
        /// <inheritdoc />
/// <author name="Sachin Baral 2025-09-19 13:01:07 +0200 12" />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_GameLobby_GameLobbyGameId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "GameLobbyUsers");

            migrationBuilder.DropIndex(
                name: "IX_Users_GameLobbyGameId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "GameLobbyGameId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "GameLobbyJoinedUsers",
                columns: table => new
                {
                    GameLobbyGameId = table.Column<Guid>(type: "uuid", nullable: false),
                    JoinedUsersUsername = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameLobbyJoinedUsers", x => new { x.GameLobbyGameId, x.JoinedUsersUsername });
                    table.ForeignKey(
                        name: "FK_GameLobbyJoinedUsers_GameLobby_GameLobbyGameId",
                        column: x => x.GameLobbyGameId,
                        principalTable: "GameLobby",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameLobbyJoinedUsers_Users_JoinedUsersUsername",
                        column: x => x.JoinedUsersUsername,
                        principalTable: "Users",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameLobby_HostUsername",
                table: "GameLobby",
                column: "HostUsername");

            migrationBuilder.CreateIndex(
                name: "IX_GameLobbyJoinedUsers_JoinedUsersUsername",
                table: "GameLobbyJoinedUsers",
                column: "JoinedUsersUsername");

            migrationBuilder.AddForeignKey(
                name: "FK_GameLobby_Users_HostUsername",
                table: "GameLobby",
                column: "HostUsername",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
/// <author name="Sachin Baral 2025-09-19 13:01:07 +0200 73" />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameLobby_Users_HostUsername",
                table: "GameLobby");

            migrationBuilder.DropTable(
                name: "GameLobbyJoinedUsers");

            migrationBuilder.DropIndex(
                name: "IX_GameLobby_HostUsername",
                table: "GameLobby");

            migrationBuilder.AddColumn<Guid>(
                name: "GameLobbyGameId",
                table: "Users",
                type: "uuid",
                nullable: true);

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
                name: "IX_Users_GameLobbyGameId",
                table: "Users",
                column: "GameLobbyGameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameLobbyUsers_Username",
                table: "GameLobbyUsers",
                column: "Username");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_GameLobby_GameLobbyGameId",
                table: "Users",
                column: "GameLobbyGameId",
                principalTable: "GameLobby",
                principalColumn: "GameId");
        }
    }
}