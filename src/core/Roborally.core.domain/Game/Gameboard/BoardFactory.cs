namespace Roborally.core.domain.Game.Gameboard;

using Space;

public static class BoardFactory {
    public static GameBoard GetEmptyBoard() {
        Space.Space[][] spaces = new Space.Space[10][];
        
        for (int i = 0; i < 10; i++) {
            spaces[i] = new Space.Space[10];
            for (int j = 0; j < 10; j++) {
                spaces[i][j] = new EmptySpace();
            }
        }

        return new GameBoard() {
            Name = "Empty Board",
            Space = spaces
        };
    }
}