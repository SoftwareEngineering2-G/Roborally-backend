using FastEndpoints;

namespace Roborally.core.application.CommandContracts.Game;

public class ActivateBoardElementCommand : ICommand
{
    public required Guid GameId { get; init; }
    public required string Username { get; init; }
}