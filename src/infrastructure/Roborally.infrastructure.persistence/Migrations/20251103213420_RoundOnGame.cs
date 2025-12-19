using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roborally.infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class RoundOnGame : Migration
    {
        /// <inheritdoc />
/// <author name="Sachin Baral 2025-11-04 21:24:56 +0100 11" />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoundCount",
                table: "Games",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
/// <author name="Sachin Baral 2025-11-04 21:24:56 +0100 22" />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoundCount",
                table: "Games");
        }
    }
}