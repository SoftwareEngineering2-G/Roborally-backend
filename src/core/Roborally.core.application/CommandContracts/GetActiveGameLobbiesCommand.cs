using FastEndpoints;
using Roborally.core.domain.Lobby;

namespace Roborally.core.application.CommandContracts;

public class GetActiveGameLobbiesCommand : ICommand<IList<GetActiveGameLobbyCommandResponse>>
{
}

public class GetActiveGameLobbyCommandResponse {
    public Guid GameId { get; init; }
    public string Name { get; init; }
    public string HostUsername { get; init; }
    public int CurrentAmountOfPlayers { get; init; }
}