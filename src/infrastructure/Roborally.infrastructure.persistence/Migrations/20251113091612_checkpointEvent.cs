using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roborally.infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class checkpointEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentCheckpointPassed",
                table: "Players",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CheckpointReachedEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    CheckpointNumber = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckpointReachedEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckpointReachedEvents_GameEvents_Id",
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
                name: "CheckpointReachedEvents");

            migrationBuilder.DropColumn(
                name: "CurrentCheckpointPassed",
                table: "Players");
        }
    }
}
