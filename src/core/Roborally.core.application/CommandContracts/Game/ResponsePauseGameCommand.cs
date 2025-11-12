using FastEndpoints;

namespace Roborally.core.application.CommandContracts.Game;

public class ResponsePauseGameCommand : ICommand {
    public required Guid GameId { get; init; }
    public required string ResponderUsername { get; init; }
    public required bool Approved { get; init; }
}