using Roborally.core.domain.Bases;

namespace Roborally.core.domain.Game;

public class GamePhase : Enumeration {

    public readonly GamePhase ProgrammingPhase = new GamePhase("ProgrammingPhase");
    public readonly GamePhase ActivationPhase = new GamePhase("ActivationPhase");


    private GamePhase(string displayName) : base(displayName) {
        
    }
}