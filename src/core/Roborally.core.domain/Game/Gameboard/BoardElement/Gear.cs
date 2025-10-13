namespace Roborally.core.domain.Game.Gameboard.BoardElement;

public class Gear : BoardElement
{
    public GearDirection Direction { get; set; }
    
    public override string Name()
    {
        return "Gear" + Direction.ToString();
    }

    public override void Activate(Player.Player player)
    {
        switch (Direction)
        {
            case GearDirection.Clockwise:
                player.RotateRight();
                break;
            case GearDirection.CounterClockwise:
                player.RotateLeft();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

public enum GearDirection
{
    Clockwise,
    CounterClockwise
}