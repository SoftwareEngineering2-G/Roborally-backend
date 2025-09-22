using System.ComponentModel.DataAnnotations.Schema;
using Roborally.core.domain.Bases;

namespace Roborally.core.domain.Game;

public class GameBoard {
    public Guid Id { get; init; } = Guid.NewGuid();

    public string[][] SpaceNames { get; set; } = Array.Empty<string[]>();


    [NotMapped]
    public Space[][] Spaces
    {
        get => SpaceNames.Select(r => r.Select(SpaceFactory.FromName).ToArray()).ToArray();
        set => SpaceNames = value.Select(r => r.Select(s => s.Name).ToArray()).ToArray();
    }

    //TODO remove just for testing
    public static GameBoard CreateEmpty(int width, int height)
    {
        var grid = new Space[height][];
        for (int r = 0; r < height; r++)
        {
            grid[r] = new Space[width];
            for (int c = 0; c < width; c++)
                grid[r][c] = new EmptySpace(); 
        }
        return new GameBoard { Spaces = grid };
    }
}

public static class SpaceFactory
{
    public static Space FromName(string name) => name switch
    {
        "empty" => new EmptySpace(),
        _ => throw new InvalidOperationException($"Unknown space '{name}'")
    };
}