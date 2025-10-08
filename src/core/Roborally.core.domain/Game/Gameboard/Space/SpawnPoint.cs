namespace Roborally.core.domain.Game.Gameboard.Space;

public class SpawnPoints : Space {
    public override string Name() {
        return "SpawnPoint";
    }
}


// suggested changes from chat:
namespace Roborally.core.domain.Game.Gameboard.Space;

public class SpawnPoint : BoardElement
{
    public int Index { get; }  // 0,1,2,… to assign players deterministically

    public SpawnPoint(int index)
    {
        Index = index;
    }

    public override string Name() => $"SpawnPoint{Index}";

    // No effect on enter; spawn/respawn handled elsewhere
    public override void OnEnter(Roborally.core.domain.Game.Player.Player player) { }
}