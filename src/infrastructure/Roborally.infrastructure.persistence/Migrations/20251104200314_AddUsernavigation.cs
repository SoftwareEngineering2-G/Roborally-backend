using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roborally.infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUsernavigation : Migration
    {
        /// <inheritdoc />
/// <author name="Sachin Baral 2025-11-04 21:24:56 +0100 11" />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
/// <author name="Sachin Baral 2025-11-04 21:24:56 +0100 33" />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}