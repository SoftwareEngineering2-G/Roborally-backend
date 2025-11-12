using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roborally.infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class PauseGameEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PauseGameEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    evokedByUsername = table.Column<string>(type: "text", nullable: false),
                    isRequest = table.Column<bool>(type: "boolean", nullable: false),
                    isAnAcceptedResponse = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PauseGameEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PauseGameEvents_GameEvents_Id",
                        column: x => x.Id,
                        principalTable: "GameEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PauseGameEvents");
        }
    }
}
