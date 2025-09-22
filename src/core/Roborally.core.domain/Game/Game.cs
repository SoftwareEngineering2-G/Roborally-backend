using Roborally.core.domain.Bases;

namespace Roborally.core.domain.Game;

public class Game {

    public Guid Id { get; init; }  = Guid.NewGuid();
    public GameBoard GameBoard { get; set; }
    public GamePhase GamePhase { get; set; }
    
    private List<Player> _players = new ();
    public IReadOnlyList<Player> Players => _players.AsReadOnly();
    
    public void AddPlayer(Player player) => _players.Add(player);
    
    //TODO maybe game owner needed for when deleting game
    //or over all who has control over the game
    
}