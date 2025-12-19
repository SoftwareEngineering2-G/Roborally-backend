using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roborally.infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class ImplementRatings : Migration
    {
        /// <inheritdoc />
/// <author name="Sachin Baral 2025-11-15 19:46:26 +0100 11" />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckpointReachedEvents_GameEvents_Id",
                table: "CheckpointReachedEvents");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "CheckpointReachedEvents");

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Winner",
                table: "Games",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoundCount",
                table: "BoardElementActivatedEvents",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckpointReachedEvents_PlayerEvents_Id",
                table: "CheckpointReachedEvents",
                column: "Id",
                principalTable: "PlayerEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
/// <author name="Sachin Baral 2025-11-15 19:46:26 +0100 51" />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckpointReachedEvents_PlayerEvents_Id",
                table: "CheckpointReachedEvents");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Winner",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "RoundCount",
                table: "BoardElementActivatedEvents");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "CheckpointReachedEvents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckpointReachedEvents_GameEvents_Id",
                table: "CheckpointReachedEvents",
                column: "Id",
                principalTable: "GameEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}