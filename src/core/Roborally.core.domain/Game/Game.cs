using Roborally.core.domain.Bases;

namespace Roborally.core.domain.Game;

public class Game {

    public Guid Id { get; set; }
    public GameBoard GameBoard { get; set; }
    public GamePhase GamePhase { get; set; }

    // Make sure the list is ordered in a way where we can also get the next player
    private List<Player> _players = new ();
    public IReadOnlyList<Player> Players => _players.AsReadOnly();
    
    public void AddPlayer(Player player) => _players.Add(player);
    
}