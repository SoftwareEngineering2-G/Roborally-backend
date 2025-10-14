using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;
using Roborally.core.domain.Game.Gameboard;
using Roborally.core.domain.Game.Gameboard.Space;
using Roborally.core.domain.Game.Player;

namespace Roborally.infrastructure.persistence.Game;

public class GameboardConfiguration : IEntityTypeConfiguration<GameBoard> {
    private class SpaceDto {
        public string Name { get; set; } = string.Empty;
        public string[] Walls { get; set; } = [];
    }

    public void Configure(EntityTypeBuilder<GameBoard> builder) {
        builder.HasKey(x => x.Name);
        
        builder.ToTable("Gameboards");

        // Configure Spaces matrix to be stored as JSON
        var compactJsonOptions = new JsonSerializerOptions
        {
            WriteIndented = false // Compact JSON without spaces/indentation
        };

        builder.Property(g => g.Spaces)
            .HasConversion(
                // Convert Spaces[][] to JSON string
                v => JsonSerializer.Serialize(
                    v.Select(row => row.Select(space => new SpaceDto {
                        Name = space.Name(),
                        Walls = space.Walls().Select(w => w.DisplayName).ToArray()
                    }).ToArray()).ToArray(),
                    compactJsonOptions),
                
                // Convert JSON string back to Spaces[][]
                v => JsonSerializer.Deserialize<SpaceDto[][]>(v, compactJsonOptions)!
                    .Select(row => row.Select(dto => 
                         SpaceFactory.FromNameAndWalls(
                             dto.Name, 
                             dto.Walls.Select(Direction.FromDisplayName).ToArray()
                         )
                    ).ToArray()).ToArray(),
                
                // Value comparer for proper change detection
                new ValueComparer<Space[][]>(
                    (c1, c2) => CompareSpaceArrays(c1, c2),
                    c => c.Aggregate(0, (a, row) => 
                        HashCode.Combine(a, row.Aggregate(0, (b, space) => 
                            HashCode.Combine(b, space.Name().GetHashCode())))),
                    c => c.Select(row => row.Select(space => SpaceFactory.FromName(space.Name())).ToArray()).ToArray()))
            .HasColumnName("SpaceMatrix")
            .HasColumnType("json");
    }


    private static bool CompareSpaceArrays(Space[][]? array1, Space[][]? array2)
    {
        if (array1 == null && array2 == null) return true;
        if (array1 == null || array2 == null) return false;
        if (array1.Length != array2.Length) return false;

        for (int i = 0; i < array1.Length; i++)
        {
            if (array1[i].Length != array2[i].Length) return false;
            for (int j = 0; j < array1[i].Length; j++)
            {
                if (array1[i][j].Name() != array2[i][j].Name()) return false;
                
                var walls1 = array1[i][j].Walls();
                var walls2 = array2[i][j].Walls();
                
                if (walls1.Length != walls2.Length) return false;
                if (!walls1.All(w => walls2.Contains(w))) return false;
            }
        }
        return true;
    }
}