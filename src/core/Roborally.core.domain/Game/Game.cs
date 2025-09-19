namespace Roborally.core.domain.Game;

public class Game {

    public GameBoard GameBoard { get; set; }
    public GamePhase GamePhase { get; set; }

    // Make sure the list is ordered in a way where we can also get the next player
    private List<Player> _players;
    public IReadOnlyList<Player> Players => _players.AsReadOnly();
    
}