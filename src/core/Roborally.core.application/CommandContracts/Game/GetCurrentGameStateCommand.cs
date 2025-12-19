using FastEndpoints;

namespace Roborally.core.application.CommandContracts.Game;

public sealed class GetCurrentGameStateCommand : ICommand<GetCurrentGameStateCommandResponse> {
    public required Guid GameId { get; init; }
    public required string Username { get; set; }
}

public sealed class GetCurrentGameStateCommandResponse {
    public required string GameId { get; set; }
    public required string HostUsername { get; set; }
    public required string Name { get; set; }
    public required bool IsPrivate { get; init; }
    public required List<Player> Players { get; set; } = [];
    public required GameBoardSpaces GameBoard { get; set; }
    public required string CurrentPhase { get; set; }
    public required int CurrentRevealedRegister { get; set; }
    public required int RoundCount { get; set; }
    public required MyState PersonalState { get; set; }
    public required string? CurrentTurnUsername { get; set; }
    public required int? CurrentExecutingRegister { get; set; }
    
/// <author name="Nilanjana Devkota 2025-10-19 11:13:58 +0200 24" />
    public record Space(string Name, List<string> Walls, string? Direction = null);

    public record Player(
        string Username,
        int CurrentRating,
        string Robot,
        int PositionX,
        int PositionY,
        string Direction,
        bool HasLockedInRegisters,
        List<string> RevealedCardsInOrder,
        int CurrentCheckpointPassed,
        int RoundCount);

/// <author name="Truong Son NGO 2025-11-28 15:36:33 +0100 38" />
    public record MyState(List<string>? LockedInCards, List<string>? DealtCards, int ProgrammingPickPilesCount, int DiscardPilesCount);
/// <author name="Sachin Baral 2025-11-04 21:24:56 +0100 39" />
    public record GameBoardSpaces(string Name, Space[][] Spaces);

}