using Roborally.core.domain.Game.Gameboard;

namespace Roborally.core.domain.Game;

public class Game {

    public Guid GameId { get; set; }

    public GameBoard GameBoard { get; set; }
    public GamePhase CurrentPhase { get; set; }

    // Make sure the list is ordered in a way where we can also get the next player
    private readonly List<Player.Player> _players;

    public IReadOnlyList<Player.Player> Players => _players.AsReadOnly();

    private Game() {
        // EFC needs the empty constructor , i know IDE warns it but please dont delete it.
    }

    public Game(Guid gameId, List<Player.Player> players, GameBoard gameBoard) {
        GameId = gameId;
        _players = players;
        GameBoard = gameBoard;
        CurrentPhase = GamePhase.ProgrammingPhase;
    }





}