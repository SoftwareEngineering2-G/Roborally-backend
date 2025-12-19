using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roborally.infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelSchema : Migration
    {
        /// <inheritdoc />
/// <author name="Suhani Pandey 2025-10-10 13:01:53 +0200 11" />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentRevealedRegister",
                table: "Games",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
/// <author name="Suhani Pandey 2025-10-10 13:01:53 +0200 22" />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentRevealedRegister",
                table: "Games");
        }
    }
}