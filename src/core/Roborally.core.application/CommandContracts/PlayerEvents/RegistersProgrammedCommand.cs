using FastEndpoints;

namespace Roborally.core.application.CommandContracts.PlayerEvents;

public class RegistersProgrammedCommand : ICommand {
    public required string Username { get; init; }
    public required Guid GameId { get; init; }
    public required List<string> LockedInCardsInOrder { get; init; }
}