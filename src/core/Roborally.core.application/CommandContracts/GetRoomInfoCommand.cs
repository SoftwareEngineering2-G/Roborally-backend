using FastEndpoints;

namespace Roborally.core.application.CommandContracts;

public class GetRoomInfoCommand : ICommand<string> {
    public string Username { get; set; }
    public Guid GameId { get; set; }
}                                                              