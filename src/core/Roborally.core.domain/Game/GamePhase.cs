using Roborally.core.domain.Bases;

namespace Roborally.core.domain.Game;

public class GamePhase : Enumeration {

    public static readonly GamePhase ProgrammingPhase = new GamePhase("ProgrammingPhase");
    public static readonly GamePhase ActivationPhase = new GamePhase("ActivationPhase");


    private GamePhase(string displayName) : base(displayName) {
        
    }
    
    public static GamePhase From(string name) => name switch
    {
        "ProgrammingPhase"   => ProgrammingPhase,
        "ActivationPhase" => ProgrammingPhase,
        _ => throw new ArgumentOutOfRangeException(nameof(name), name, "Unknown GamePhase")
    };
}