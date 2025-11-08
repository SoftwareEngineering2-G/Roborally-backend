using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roborally.infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class PauseGameState : Migration
    {
        /// <inheritdoc />
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
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isPaused",
                table: "Games");
        }
    }
}
