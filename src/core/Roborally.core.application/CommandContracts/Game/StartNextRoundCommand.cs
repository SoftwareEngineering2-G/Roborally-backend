using FastEndpoints;

namespace Roborally.core.application.CommandContracts.Game;

public class StartNextRoundCommand : ICommand {
    public Guid GameId { get; set; }
}

