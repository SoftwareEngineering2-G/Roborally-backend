using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roborally.infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class PauseGameState : Migration
    {
        /// <inheritdoc />
/// <author name="Truong Son NGO 2025-11-12 15:35:28 +0100 11" />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isPaused",
                table: "Games",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
/// <author name="Truong Son NGO 2025-11-12 15:35:28 +0100 22" />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isPaused",
                table: "Games");
        }
    }
}