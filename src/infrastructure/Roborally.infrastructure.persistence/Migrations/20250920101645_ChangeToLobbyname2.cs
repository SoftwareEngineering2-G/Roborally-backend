using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roborally.infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeToLobbyname2 : Migration
    {
        /// <inheritdoc />
/// <author name="Sachin Baral 2025-09-20 20:52:08 +0200 11" />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GameRoomName",
                table: "GameLobby",
                newName: "LobbyName");
        }

        /// <inheritdoc />
/// <author name="Sachin Baral 2025-09-20 20:52:08 +0200 20" />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LobbyName",
                table: "GameLobby",
                newName: "GameRoomName");
        }
    }
}