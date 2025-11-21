using Roborally.core.domain.Game.Deck;

namespace Roborally.core.domain.Game.CardActions;

public class CardExecutionContext
{
    public string? TargetPlayerUsername { get; init; }
    public ProgrammingCard? SelectedMovementCard { get; init; }
}
