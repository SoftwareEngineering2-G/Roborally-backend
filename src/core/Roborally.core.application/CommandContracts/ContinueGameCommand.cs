using FastEndpoints;

namespace Roborally.core.application.CommandContracts;

public class ContinueGameCommand : ICommand{
    public required string Username { get; init; }
    public required Guid GameId { get; init; }
}