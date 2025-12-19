using Roborally.core.domain.Game.Player;

namespace Roborally.core.domain.Game.Gameboard.Space;

public class Checkpoint : Space
{
    public required int CheckpointNumber { get; init; }
    
/// <author name="Suhani Pandey 2025-11-13 16:20:36 +0100 9" />
    public Checkpoint(Direction[]? walls = null) : base(walls) {}
    
/// <author name="Suhani Pandey 2025-11-13 16:20:36 +0100 11" />
    public override string Name()
    {
        return $"Checkpoint{CheckpointNumber}";
    }
}