using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roborally.infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCardExecutedEvents : Migration
    {
        /// <inheritdoc />
/// <author name="Suhani Pandey 2025-10-15 21:47:56 +0200 11" />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastExecutedCardName",
                table: "Players");

            migrationBuilder.CreateTable(
                name: "CardExecutedEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    CardName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardExecutedEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardExecutedEvents_PlayerEvents_Id",
                        column: x => x.Id,
                        principalTable: "PlayerEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
/// <author name="Suhani Pandey 2025-10-15 21:47:56 +0200 37" />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardExecutedEvents");

            migrationBuilder.AddColumn<string>(
                name: "LastExecutedCardName",
                table: "Players",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}