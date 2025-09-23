namespace Roborally.core.domain.Game;

public class Game {

    public Guid GameId { get; set; }

    public GameBoard GameBoard { get; set; }
    public GamePhase CurrentPhase { get; set; }

    // Make sure the list is ordered in a way where we can also get the next player
    private List<Player> _players;

    public IReadOnlyList<Player> Players => _players.AsReadOnly();

    private Game() {
        // EFC needs the empty constructor if its private, i know IDE warns it but please dont delete it.
    }

    private Game(Guid gameId, GameBoard gameBoard, List<Player> players) {
        GameBoard = gameBoard;
        GameId = gameId;
        _players = players;
    }
}