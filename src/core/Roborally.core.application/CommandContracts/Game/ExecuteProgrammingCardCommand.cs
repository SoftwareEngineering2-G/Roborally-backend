using FastEndpoints;

namespace Roborally.core.application.CommandContracts.Game;

public class ExecuteProgrammingCardCommand : ICommand<ExecuteProgrammingCardCommandResponse>
{
    public required Guid GameId { get; init; }
    public required string Username { get; init; }
    public required string CardName { get; init; }
}

public class ExecuteProgrammingCardCommandResponse
{
    public required string Message { get; init; }
    public required PlayerState PlayerState { get; init; }
}

public class PlayerState
{
    public required int PositionX { get; init; }
    public required int PositionY { get; init; }
    public required string Direction { get; init; }
}

