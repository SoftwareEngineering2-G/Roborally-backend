using FastEndpoints;

namespace Roborally.core.application.CommandContracts.Game;

public class RevealNextRegisterCommand : ICommand<RevealNextRegisterResult>
{
    public required Guid GameId { get; init; }
    public required string Username { get; init; }
}

public class RevealNextRegisterResult
{
    public required int RegisterNumber { get; init; }
    public required List<PlayerRevealedCard> RevealedCards { get; init; }
}

public class PlayerRevealedCard
{
    public required string Username { get; init; }
    public required string Card { get; init; }
}
