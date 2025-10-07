namespace Roborally.core.domain.Game.Gameboard;

using Space;

public static class BoardFactory {
    private static readonly Lock Lock = new ();
    private static GameBoard? _emptyBoard;

    public static GameBoard GetEmptyBoard() {
        if (_emptyBoard is not null) {
            return _emptyBoard;
        }

        lock (Lock) {
            // Double-check locking pattern to ensure thread safety
            if (_emptyBoard is not null) {
                return _emptyBoard;
            }

            Space.Space[][] spaces = new Space.Space[10][];
            
            for (int i = 0; i < 10; i++) {
                spaces[i] = new Space.Space[10];
                for (int j = 0; j < 10; j++) {
                    spaces[i][j] = new EmptySpace();
                }
            }
            // hardcoded checkpoints
            spaces[2][2] = new Checkpoint(1);
            spaces[5][5] = new Checkpoint(2);
            spaces[4][4] = new Checkpoint(3);
            spaces[8][8] = new Checkpoint(4);
            spaces[9][9] = new Checkpoint(5);
            spaces[3][9] = new Checkpoint(6);

            _emptyBoard = new GameBoard() {
                Name = "Empty Board",
                Space = spaces
            };
        }

        return _emptyBoard;
    }
}
