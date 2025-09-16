using FastEndpoints;

namespace Roborally.core.application.Contracts;

public class CreateGameLobbyCommand : ICommand<Guid>
{
     public required Guid HostUserId { get; set; }
     public required bool IsPrivate { get; set; }
     public required string GameRoomName { get; set; }
}