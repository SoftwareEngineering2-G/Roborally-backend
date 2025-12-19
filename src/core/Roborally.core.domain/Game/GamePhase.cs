using Roborally.core.domain.Bases;

namespace Roborally.core.domain.Game;

public class GamePhase : Enumeration {

/// <author name="Sachin Baral 2025-09-19 17:07:35 +0200 7" />
    public static readonly GamePhase ProgrammingPhase = new GamePhase("ProgrammingPhase");
/// <author name="Sachin Baral 2025-09-19 17:07:35 +0200 8" />
    public static readonly GamePhase ActivationPhase = new GamePhase("ActivationPhase");
/// <author name="Andrej Jurco 2025-11-14 10:12:24 +0100 9" />
    public static readonly GamePhase GameOver = new GamePhase("GameOver");


/// <author name="Sachin Baral 2025-09-19 17:07:35 +0200 12" />
    private GamePhase(string displayName) : base(displayName) {
        
    }
}