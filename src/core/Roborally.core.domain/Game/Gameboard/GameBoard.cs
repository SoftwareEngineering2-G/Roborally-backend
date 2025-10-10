namespace Roborally.core.domain.Game.Gameboard;

public class GameBoard {
    public required string Name { get; set; }
    public required Space.Space[][] Space { get; init; }

    internal GameBoard() {

    }
    
    public Space.Space GetSpaceAt(int x, int y) {
        if (y < 0 || y >= Space.Length || x < 0 || x >= Space[0].Length)
            throw new ArgumentOutOfRangeException();
        return Space[y][x];
    }
}