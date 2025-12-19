using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roborally.infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class Updategane : Migration
    {
        /// <inheritdoc />
/// <author name="Sachin Baral 2025-10-01 21:53:45 +0200 11" />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HostUsername",
                table: "Games",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Games",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Games_HostUsername",
                table: "Games",
                column: "HostUsername");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Users_HostUsername",
                table: "Games",
                column: "HostUsername",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
/// <author name="Sachin Baral 2025-10-01 21:53:45 +0200 44" />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Users_HostUsername",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_HostUsername",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "HostUsername",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Games");
        }
    }
}