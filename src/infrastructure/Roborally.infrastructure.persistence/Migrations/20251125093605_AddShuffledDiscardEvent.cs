using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roborally.infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddShuffledDiscardEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiscardPilesShuffledIntoPickPilesEvent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    NewPickPiles = table.Column<string>(type: "json", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscardPilesShuffledIntoPickPilesEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscardPilesShuffledIntoPickPilesEvent_PlayerEvents_Id",
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
                name: "DiscardPilesShuffledIntoPickPilesEvent");
        }
    }
}
