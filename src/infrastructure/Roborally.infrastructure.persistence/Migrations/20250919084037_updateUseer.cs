using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roborally.infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class updateUseer : Migration
    {
        /// <inheritdoc />
/// <author name="Sachin Baral 2025-09-19 13:01:07 +0200 12" />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameLobby_Users_HostId",
                table: "GameLobby");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Username",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_GameLobby_HostId",
                table: "GameLobby");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HostId",
                table: "GameLobby");

            migrationBuilder.AddColumn<Guid>(
                name: "GameLobbyGameId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HostUsername",
                table: "GameLobby",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_Users_GameLobbyGameId",
                table: "Users",
                column: "GameLobbyGameId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Users_GameLobby_GameLobbyGameId",
                table: "Users",
                column: "GameLobbyGameId",
                principalTable: "GameLobby",
                principalColumn: "GameId");
        }

        /// <inheritdoc />
/// <author name="Sachin Baral 2025-09-19 13:01:07 +0200 84" />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameLobby_Users_HostUsername",
                table: "GameLobby");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_GameLobby_GameLobbyGameId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_GameLobbyGameId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_GameLobby_HostUsername",
                table: "GameLobby");

            migrationBuilder.DropColumn(
                name: "GameLobbyGameId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HostUsername",
                table: "GameLobby");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "HostId",
                table: "GameLobby",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameLobby_HostId",
                table: "GameLobby",
                column: "HostId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GameLobby_Users_HostId",
                table: "GameLobby",
                column: "HostId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}