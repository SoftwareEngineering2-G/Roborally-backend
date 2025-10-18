using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;
using Roborally.core.domain.Game.Gameboard;
using Roborally.core.domain.Game.Gameboard.Space;
using Roborally.core.domain.Game.Gameboard.BoardElement;
using Roborally.core.domain.Game.Player;

namespace Roborally.infrastructure.persistence.Game;

public class GameboardConfiguration : IEntityTypeConfiguration<GameBoard> {
    private class SpaceDto {
        public string Name { get; set; } = string.Empty;
        public string[] Walls { get; set; } = [];
        public string? Direction { get; set; } // For conveyor belts and gears
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
                    v.Select(row => row.Select(space => SpaceToDto(space)).ToArray()).ToArray(),
                    compactJsonOptions),
                
                // Convert JSON string back to Spaces[][]
                v => JsonSerializer.Deserialize<SpaceDto[][]>(v, compactJsonOptions)!
                    .Select(row => row.Select(dto => DtoToSpace(dto)).ToArray()).ToArray(),
                
                // Value comparer for proper change detection
                new ValueComparer<Space[][]>(
                    (c1, c2) => CompareSpaceArrays(c1, c2),
                    c => c.Aggregate(0, (a, row) => 
                        HashCode.Combine(a, row.Aggregate(0, (b, space) => 
                            HashCode.Combine(b, space.Name().GetHashCode())))),
                    c => c.Select(row => row.Select(space => CloneSpace(space)).ToArray()).ToArray()))
            .HasColumnName("SpaceMatrix")
            .HasColumnType("json");
    }

    private static SpaceDto SpaceToDto(Space space)
    {
        var dto = new SpaceDto
        {
            Name = space.Name(),
            Walls = space.Walls().Select(w => w.DisplayName).ToArray()
        };

        // Handle board elements with additional properties - use single Direction field for both
        switch (space)
        {
            case BlueConveyorBelt blueBelt:
                dto.Direction = blueBelt.Direction.DisplayName;
                break;
            case GreenConveyorBelt greenBelt:
                dto.Direction = greenBelt.Direction.DisplayName;
                break;
            case Gear gear:
                dto.Direction = gear.Direction.DisplayName;
                break;
        }

        return dto;
    }

    private static Space DtoToSpace(SpaceDto dto)
    {
        var walls = dto.Walls.Select(Direction.FromDisplayName).ToArray();

        // Handle board elements with additional properties
        switch (dto.Name)
        {
            case "BlueConveyorBelt":
                return BoardElementFactory.BlueConveyorBelt(
                    Direction.FromDisplayName(dto.Direction ?? "North"),
                    walls);
            
            case "GreenConveyorBelt":
                return BoardElementFactory.GreenConveyorBelt(
                    Direction.FromDisplayName(dto.Direction ?? "North"),
                    walls);
            
            case "Gear":
                var gearDir = dto.Direction == "ClockWise" 
                    ? GearDirection.ClockWise 
                    : GearDirection.AntiClockWise;
                return BoardElementFactory.Gear(gearDir, walls);
            
            default:
                // Fall back to SpaceFactory for regular spaces
                return SpaceFactory.FromNameAndWalls(dto.Name, walls);
        }
    }

    private static Space CloneSpace(Space space)
    {
        // Clone a space for the value comparer
        var dto = SpaceToDto(space);
        return DtoToSpace(dto);
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
                if (!CompareSpaces(array1[i][j], array2[i][j])) return false;
            }
        }
        return true;
    }

    private static bool CompareSpaces(Space space1, Space space2)
    {
        if (space1.Name() != space2.Name()) return false;
        
        var walls1 = space1.Walls();
        var walls2 = space2.Walls();
        
        if (walls1.Length != walls2.Length) return false;
        if (!walls1.All(w => walls2.Contains(w))) return false;

        // Compare board element properties
        switch (space1)
        {
            case BlueConveyorBelt blueBelt1 when space2 is BlueConveyorBelt blueBelt2:
                return blueBelt1.Direction.Equals(blueBelt2.Direction);
            
            case GreenConveyorBelt greenBelt1 when space2 is GreenConveyorBelt greenBelt2:
                return greenBelt1.Direction.Equals(greenBelt2.Direction);
            
            case Gear gear1 when space2 is Gear gear2:
                return gear1.Direction.Equals(gear2.Direction);
        }

        return true;
    }
}