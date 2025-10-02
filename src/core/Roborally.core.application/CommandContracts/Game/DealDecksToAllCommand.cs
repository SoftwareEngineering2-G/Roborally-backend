using FastEndpoints;

namespace Roborally.core.application.CommandContracts.Game;

public class DealDecksToAllCommand : ICommand{

    public required Guid GameId { get; init; }
    public required string HostUsername { get; init; }
    
}