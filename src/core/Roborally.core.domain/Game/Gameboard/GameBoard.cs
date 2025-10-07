namespace Roborally.core.domain.Game.Gameboard;

public class GameBoard {
    public required string Name { get; set; }
    public required Space.Space[][] Space { get; init; }
    public int FinalCheckpointOrder { get; init; }  // checkpoint
    internal GameBoard() {

    }
}