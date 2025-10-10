using FastEndpoints;
using Roborally.core.domain.Game.Gameboard;

namespace Roborally.core.application.CommandContracts.Game;

public class GetCurrentGameStateCommand : ICommand<GetCurrentGameStateCommandResponse> {
    public required Guid GameId { get; init; }
}

public class GetCurrentGameStateCommandResponse {
    public required string GameId { get; init; }
    public required List<Player> Players { get; init; } = [];
    public required string CurrentPhase { get; init; }
    
    public required string HostUsername { get; init; }

    public required string Name { get; init; }

    // TODO:  We probably need information about gameboards, current positions and stuff
    public GameBoardSpaces GameBoard { get; init; }
    
    public record GameBoardSpaces(string Name, Space[][] Spaces);

    public record Space(string Name);

    public record Player(
        string Username, 
        string Robot, 
        List<string>? ProgrammedCards = null,
        int PositionX = 0,
        int PositionY = 0,
        string Direction = "North"
    );
}