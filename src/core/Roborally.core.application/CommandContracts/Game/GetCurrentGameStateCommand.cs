using FastEndpoints;
using Roborally.core.domain.Game.Gameboard.BoardElement;
using Roborally.core.domain.Game.Gameboard.Space;

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
    public required GameBoardSpaces GameBoard { get; init; }
    
    public record ResponseSpace
    {
        public string Name { get; init; }
        public List<string> Walls { get; init; }
        public string? Direction { get; init; }

        public ResponseSpace(Space space)
        {
            Name = space.Name();
            Walls = space.Walls().Select(d => d.DisplayName).ToList();
            Direction = null;
            
            if (space is GreenConveyorBelt greenConveyorBelt)
            {
                Direction = greenConveyorBelt.Direction.DisplayName;
            }
            else if (space is BlueConveyorBelt blueConveyorBelt)
            {
                Direction = blueConveyorBelt.Direction.DisplayName;
            }
            else if (space is Gear gear)
            {
                Direction = gear.Direction.DisplayName;
            }
        }
    }
    
    public record GameBoardSpaces(string Name, ResponseSpace[][] Spaces);

    public record Player(
        string Username, 
        string Robot, 
        List<string>? ProgrammedCards = null,
        int PositionX = 0,
        int PositionY = 0,
        string Direction = "North"
    );
}