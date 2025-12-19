using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roborally.infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class user : Migration
    {
        /// <inheritdoc />
/// <author name="Sachin Baral 2025-11-15 19:46:26 +0100 11" />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Users_Username1",
                table: "Players");

            migrationBuilder.AlterColumn<string>(
                name: "Username1",
                table: "Players",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Users_Username1",
                table: "Players",
                column: "Username1",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
/// <author name="Sachin Baral 2025-11-15 19:46:26 +0100 37" />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Users_Username1",
                table: "Players");

            migrationBuilder.AlterColumn<string>(
                name: "Username1",
                table: "Players",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Users_Username1",
                table: "Players",
                column: "Username1",
                principalTable: "Users",
                principalColumn: "Username");
        }
    }
}