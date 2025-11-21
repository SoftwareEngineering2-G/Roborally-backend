using FastEndpoints;

namespace Roborally.core.application.CommandContracts.Game;

public class ExecuteProgrammingCardCommand : ICommand
{
    public required Guid GameId { get; init; }
    public required string Username { get; init; }
    public required string CardName { get; init; }
    public ExecuteProgrammingCardInteractiveInput? InteractiveInput { get; init; }
}

public class ExecuteProgrammingCardInteractiveInput
{
    public string? TargetPlayerUsername { get; init; }
    public string? SelectedMovementCard { get; init; }
}
