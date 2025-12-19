using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roborally.infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLastExecutedAction : Migration
    {
        /// <inheritdoc />
/// <author name="Suhani Pandey 2025-10-10 13:01:53 +0200 11" />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastExecutedCardName",
                table: "Players",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
/// <author name="Suhani Pandey 2025-10-10 13:01:53 +0200 22" />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastExecutedCardName",
                table: "Players");
        }
    }
}