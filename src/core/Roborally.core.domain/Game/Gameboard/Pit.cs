namespace Roborally.core.domain.Game.Gameboard;

public class Pit : BoardElement
{
    public override string Name() => "Pit";

    public override void OnEnter(Player player)
    {
        player.Destroy();  // or mark them as destroyed
    }
}