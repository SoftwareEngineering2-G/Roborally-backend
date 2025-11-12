using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.Gameboard.Space;

public class CheckpointSpace : Space
{
    public required int CheckpointNumber { get; init; }
    
    public override string Name()
    {
        return $"Checkpoint{CheckpointNumber}";
    }
    
    internal CheckpointSpace() : base() { }
    
    internal CheckpointSpace(Direction[]? walls) : base(walls) { }
}

