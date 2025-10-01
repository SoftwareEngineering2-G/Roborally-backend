using FastEndpoints;

namespace Roborally.core.application.CommandContracts.Game;

public class GetCurrentGameStateCommand : ICommand<GetCurrentGameStateCommandResponse> {
    public required Guid GameId { get; init; }
}

public class GetCurrentGameStateCommandResponse {
    public required string GameId { get; init; }
    public required List<Player> Players { get; init; } = [];
    public required string CurrentPhase { get; set; }

    // TODO:  We probably need information about gameboards, current positions and stuff

    public record Player(string Username, string Robot);
}