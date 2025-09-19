using FastEndpoints;

namespace Roborally.core.application.CommandContracts;

public class JoinLobbyCommand : ICommand{
    public required string Username{ get; set; }
    public required Guid GameId { get; set; }
}