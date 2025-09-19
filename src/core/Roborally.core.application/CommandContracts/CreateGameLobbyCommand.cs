using FastEndpoints;

namespace Roborally.core.application.CommandContracts;

public class CreateGameLobbyCommand : ICommand<Guid>
{
    public required string HostUsername { get; set; }
    public required bool IsPrivate { get; set; }
    public required string GameRoomName { get; set; }
}