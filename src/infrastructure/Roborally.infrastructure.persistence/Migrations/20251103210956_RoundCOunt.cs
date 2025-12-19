using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roborally.infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class RoundCOunt : Migration
    {
        /// <inheritdoc />
/// <author name="Sachin Baral 2025-11-04 21:24:56 +0100 11" />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoundCount",
                table: "Players",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Round",
                table: "PlayerEvents",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
/// <author name="Sachin Baral 2025-11-04 21:24:56 +0100 29" />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoundCount",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Round",
                table: "PlayerEvents");
        }
    }
}