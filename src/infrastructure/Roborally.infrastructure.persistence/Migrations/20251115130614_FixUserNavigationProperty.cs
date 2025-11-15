using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roborally.infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixUserNavigationProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Users_Username1",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_Username1",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Username1",
                table: "Players");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username1",
                table: "Players",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_Username1",
                table: "Players",
                column: "Username1");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Users_Username1",
                table: "Players",
                column: "Username1",
                principalTable: "Users",
                principalColumn: "Username");
        }
    }
}
