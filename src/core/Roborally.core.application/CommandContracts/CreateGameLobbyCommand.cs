using FastEndpoints;

namespace Roborally.core.application.CommandContracts;

public class CreateGameLobbyCommand : ICommand<Guid>
{
    public required string HostUsername { get; init; }
    public required bool IsPrivate { get; init; }
    public required string GameRoomName { get; init; }
}