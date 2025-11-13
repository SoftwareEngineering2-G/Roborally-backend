using Roborally.core.domain.Bases;

namespace Roborally.core.domain.Game;

public class GamePhase : Enumeration {

    public static readonly GamePhase ProgrammingPhase = new GamePhase("ProgrammingPhase");
    public static readonly GamePhase ActivationPhase = new GamePhase("ActivationPhase");
    public static readonly GamePhase GameOver = new GamePhase("GameOver");


    private GamePhase(string displayName) : base(displayName) {
        
    }
}
