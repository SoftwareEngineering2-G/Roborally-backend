using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roborally.infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Gameboards_GameBoardName",
                table: "Games");

            migrationBuilder.AlterColumn<string>(
                name: "GameBoardName",
                table: "Games",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Gameboards_GameBoardName",
                table: "Games",
                column: "GameBoardName",
                principalTable: "Gameboards",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Gameboards_GameBoardName",
                table: "Games");

            migrationBuilder.AlterColumn<string>(
                name: "GameBoardName",
                table: "Games",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Gameboards_GameBoardName",
                table: "Games",
                column: "GameBoardName",
                principalTable: "Gameboards",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
