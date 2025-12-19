using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roborally.infrastructure.persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddWalls : Migration
    {
        /// <inheritdoc />
/// <author name="Truong Son NGO 2025-10-13 13:29:29 +0200 11" />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE ""Gameboards"" AS g
                SET ""SpaceMatrix"" = sub.space_matrix_with_walls
                FROM (SELECT ""Name"",
                             (SELECT json_agg(row_objs ORDER BY row_ord)
                              FROM (SELECT row_ord,
                                           json_agg(
                                                   json_build_object('Name', cell, 'Walls', '[]'::json)
                                                   ORDER BY cell_ord
                                           ) AS row_objs
                                    FROM json_array_elements(""SpaceMatrix"") WITH ORDINALITY AS row(row_json, row_ord)
                                             CROSS JOIN LATERAL json_array_elements_text(row.row_json) WITH ORDINALITY AS cell(cell, cell_ord)
                                    GROUP BY row_ord) AS rows_by_ord) AS space_matrix_with_walls
                      FROM ""Gameboards"") AS sub
                WHERE g.""Name"" = sub.""Name""
            ");
        }

        /// <inheritdoc />
/// <author name="Truong Son NGO 2025-10-13 13:29:29 +0200 32" />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE ""Gameboards"" AS g
                SET ""SpaceMatrix"" = sub.name_matrix
                FROM (
                         SELECT
                             ""Name"",
                             (
                                 SELECT json_agg(name_row ORDER BY row_ord)
                                 FROM (
                                          SELECT row_ord,
                                                 json_agg(cell->>'Name' ORDER BY cell_ord) AS name_row
                                          FROM json_array_elements(""SpaceMatrix"") WITH ORDINALITY AS row(row_json, row_ord)
                                                   CROSS JOIN LATERAL json_array_elements(row.row_json) WITH ORDINALITY AS cell(cell, cell_ord)
                                          GROUP BY row_ord
                                      ) AS rows_by_ord
                             ) AS name_matrix
                         FROM ""Gameboards""
                     ) AS sub
                WHERE g.""Name"" = sub.""Name"";
            ");
        }
    }
}