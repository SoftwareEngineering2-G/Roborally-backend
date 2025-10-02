using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roborally.infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class CardsDealtEvents : Migration
    {
        /// <inheritdoc />
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
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProgrammingCardsDealtEvents");
        }
    }
}
