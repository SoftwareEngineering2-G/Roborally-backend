using FastEndpoints;

namespace Roborally.core.application.CommandContracts;

public class EndGameCommand:ICommand
{
    public required Guid GameId { get; init; }
}