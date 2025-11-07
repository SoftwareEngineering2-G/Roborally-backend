using FastEndpoints;

namespace Roborally.core.application.CommandContracts.Game;

public class RequestPauseGameCommand : ICommand {
    public required Guid GameId { get; init; }
    public required string RequesterUsername { get; init; }
}