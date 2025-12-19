using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roborally.infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPauseGameState : Migration
    {
        /// <inheritdoc />
/// <author name="Truong Son NGO 2025-11-12 15:35:28 +0100 11" />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isPaused",
                table: "Games",
                newName: "IsPaused");
        }

        /// <inheritdoc />
/// <author name="Truong Son NGO 2025-11-12 15:35:28 +0100 20" />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPaused",
                table: "Games",
                newName: "isPaused");
        }
    }
}