using FastEndpoints;

namespace Roborally.core.application.CommandContracts;

public class LeaveLobbyCommand : ICommand
{
    public required string Username { get; init; }
    public required Guid GameId { get; init; }
}