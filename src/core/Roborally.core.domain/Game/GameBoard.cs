namespace Roborally.core.domain.Game;

public class GameBoard {
    public Space[][] Space { get; init; }
    
    //TODO remove just for testing
    public static GameBoard CreateEmpty(int width, int height)
    {
        var grid = new Space[height][];
        for (int r = 0; r < height; r++)
        {
            grid[r] = new Space[width];
            for (int c = 0; c < width; c++)
                grid[r][c] = new EmptySpace(); // or Space.Empty
        }
        return new GameBoard { Space = grid };
    }
}