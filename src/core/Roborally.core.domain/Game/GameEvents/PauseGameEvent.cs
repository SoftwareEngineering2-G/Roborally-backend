namespace Roborally.core.domain.Game.GameEvents;

public class PauseGameEvent : GameEvent {
    public required string evokedByUsername { get; set; }
    public required bool isRequest { get; set; }
    public bool? isAnAcceptedResponse { get; set; }
}