using FastEndpoints;

namespace Roborally.core.application.CommandContracts;

public class StartGameCommand : ICommand{
    public required string Username { get; init; }
    public required Guid GameId { get; init; }
    public required string GameBoardName { get; init; }
}