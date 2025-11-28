using System.Text.Json;
using Roborally.core.domain.Game.Gameboard.Space;
using Roborally.core.domain.Game.Gameboard.BoardElement;
using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.Gameboard;

public static class GameBoardJsonLoader
{
    private class SpaceDto
    {
        public string Name { get; set; } = string.Empty;
        public string[] Walls { get; set; } = [];
        public string? Direction { get; set; }
    }

    public static GameBoard LoadFromJson(string jsonFilePath, string boardName)
    {
        if (!File.Exists(jsonFilePath))
        {
            throw new FileNotFoundException($"Game board JSON file not found: {jsonFilePath}");
        }

        string jsonContent = File.ReadAllText(jsonFilePath);
        var spaceDtos = JsonSerializer.Deserialize<SpaceDto[][]>(jsonContent);

        if (spaceDtos == null)
        {
            throw new InvalidOperationException($"Failed to deserialize game board from: {jsonFilePath}");
        }

        Space.Space[][] spaces = spaceDtos
            .Select(row => row.Select(DtoToSpace).ToArray())
            .ToArray();

        return new GameBoard
        {
            Name = boardName,
            Spaces = spaces
        };
    }

    private static Space.Space DtoToSpace(SpaceDto dto)
    {
        var walls = dto.Walls.Select(Direction.FromDisplayName).ToArray();

        return dto.Name switch
        {
            "BlueConveyorBelt" => BoardElementFactory.BlueConveyorBelt(
                Direction.FromDisplayName(dto.Direction ?? "North"),
                walls),
            
            "GreenConveyorBelt" => BoardElementFactory.GreenConveyorBelt(
                Direction.FromDisplayName(dto.Direction ?? "North"),
                walls),
            
            "Gear" => BoardElementFactory.Gear(
                dto.Direction == "ClockWise" 
                    ? GearDirection.ClockWise 
                    : GearDirection.AntiClockWise, 
                walls),
            
            "PriorityAntenna" => BoardElementFactory.PriorityAntenna(walls),
            
            var name when name.StartsWith("Checkpoint") => ParseCheckpoint(dto.Name, walls),
            
            _ => SpaceFactory.FromNameAndWalls(dto.Name, walls)
        };
    }
    
    private static Space.Space ParseCheckpoint(string name, Direction[] walls)
    {
        // Extract checkpoint number from name like "Checkpoint1", "Checkpoint2", etc.
        string numberPart = name.Replace("Checkpoint", "");
        if (int.TryParse(numberPart, out int checkpointNumber))
        {
            return SpaceFactory.Checkpoint(checkpointNumber, walls);
        }

        throw new InvalidOperationException($"Invalid checkpoint name: {name}. Expected format: Checkpoint1, Checkpoint2, etc.");
    }
}
