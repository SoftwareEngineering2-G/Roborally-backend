using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Roborally.infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class playerEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Username = table.Column<string>(type: "text", nullable: false),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrentFacingDirection = table.Column<string>(type: "text", nullable: false),
                    CurrentPositionX = table.Column<int>(type: "integer", nullable: false),
                    CurrentPositionY = table.Column<int>(type: "integer", nullable: false),
                    DiscardedPiles = table.Column<string>(type: "json", nullable: false),
                    PickPiles = table.Column<string>(type: "json", nullable: false),
                    Robot = table.Column<string>(type: "text", nullable: false),
                    SpawnPositionX = table.Column<int>(type: "integer", nullable: false),
                    SpawnPositionY = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => new { x.GameId, x.Username });
                });

            migrationBuilder.CreateTable(
                name: "PlayerEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    PlayerGameId = table.Column<Guid>(type: "uuid", nullable: true),
                    PlayerUsername = table.Column<string>(type: "text", nullable: true),
                    HappenedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerEvents_Players_PlayerGameId_PlayerUsername",
                        columns: x => new { x.PlayerGameId, x.PlayerUsername },
                        principalTable: "Players",
                        principalColumns: new[] { "GameId", "Username" });
                });

            migrationBuilder.CreateTable(
                name: "RegisterProgrammedEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    ProgrammedCardsInOrder = table.Column<string>(type: "json", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisterProgrammedEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegisterProgrammedEvents_PlayerEvents_Id",
                        column: x => x.Id,
                        principalTable: "PlayerEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerEvents_PlayerGameId_PlayerUsername",
                table: "PlayerEvents",
                columns: new[] { "PlayerGameId", "PlayerUsername" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegisterProgrammedEvents");

            migrationBuilder.DropTable(
                name: "PlayerEvents");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
