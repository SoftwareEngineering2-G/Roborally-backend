using FastEndpoints;

namespace Roborally.core.application.CommandContracts;

public class StartGameCommand : ICommand{
    public string Username { get; set; }
    public Guid GameId { get; set; }
}