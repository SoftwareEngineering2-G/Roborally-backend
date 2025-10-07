namespace Roborally.core.domain.Game.Gameboard.Space;

using Roborally.core.domain.Game.Player;

// Checkpoint is a special board element
public class Checkpoint : BoardElement
{
    public int Order { get; }   // which checkpoint number (1, 2, 3, …)

    public Checkpoint(int order)
    {
        Order = order;          // you set the number when you create it
    }

    // This is just its label
    public override string Name() => $"Checkpoint{Order}";

    // Runs whenever a player steps on this space
    public override void OnEnter(Player player)
    {
        // Only allow sequential progress
        if (player.CheckpointIndex + 1 == Order)
        {
            player.CheckpointIndex = Order;  // update their progress
        }
    }
}