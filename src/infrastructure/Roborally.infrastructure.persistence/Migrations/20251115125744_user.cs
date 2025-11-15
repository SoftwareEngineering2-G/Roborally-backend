using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roborally.infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class user : Migration
    {
        /// <inheritdoc />
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
