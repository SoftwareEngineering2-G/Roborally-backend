using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roborally.infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class CardsDealtEvents : Migration
    {
        /// <inheritdoc />
/// <author name="Sachin Baral 2025-10-01 21:53:45 +0200 11" />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProgrammingCardsDealtEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    DealtCards = table.Column<string>(type: "json", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgrammingCardsDealtEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgrammingCardsDealtEvents_PlayerEvents_Id",
                        column: x => x.Id,
                        principalTable: "PlayerEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
/// <author name="Sachin Baral 2025-10-01 21:53:45 +0200 33" />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProgrammingCardsDealtEvents");
        }
    }
}