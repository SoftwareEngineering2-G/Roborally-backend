using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.Gameboard.Space;

public class Checkpoint : Space
{
    public required int CheckpointNumber { get; init; }
    
    public Checkpoint(Direction[]? walls = null) : base(walls) {}
    
    public override string Name()
    {
        return $"Checkpoint{CheckpointNumber}";
    }
}