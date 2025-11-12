using Roborally.core.domain.Bases;

namespace Roborally.core.domain.Game;

public class GamePauseState {
    public bool result { get; set; }
    public string RequestedBy { get; set ; }
    public Dictionary<string, bool> PlayerResponses { get; set; }
    public DateTime RequestedAt { get; set; }
}
