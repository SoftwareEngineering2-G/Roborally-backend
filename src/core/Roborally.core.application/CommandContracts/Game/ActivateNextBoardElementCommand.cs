using FastEndpoints;

namespace Roborally.core.application.CommandContracts.Game;

public class ActivateNextBoardElementCommand : ICommand {
    public required Guid GameId { get; init; }
}