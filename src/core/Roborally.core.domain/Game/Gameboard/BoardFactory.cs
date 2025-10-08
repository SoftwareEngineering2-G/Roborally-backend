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
            // Spawnpoints 1-6
            spaces[0][0] = new SpawnPoint(1);
            spaces[0][9] = new SpawnPoint(2);
            spaces[9][0] = new SpawnPoint(3);
            spaces[0][5] = new SpawnPoint(4);
            spaces[5][0] = new SpawnPoint(5);
            spaces[2][0] = new SpawnPoint(6);

            _emptyBoard = new GameBoard() {
                Name = "Empty Board",
                Space = spaces
            };
        }
        return _emptyBoard;
    }
}
