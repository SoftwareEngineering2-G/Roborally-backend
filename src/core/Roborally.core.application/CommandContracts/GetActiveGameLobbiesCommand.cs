using FastEndpoints;
using Roborally.core.domain.Lobby;

namespace Roborally.core.application.CommandContracts;

public class GetActiveGameLobbiesCommand : ICommand<IList<GetActiveGameLobbyCommandResponse>>
{
}

public class GetActiveGameLobbyCommandResponse {
    public Guid GameId { get; set; }
    public string Name { get; set; }
    public string HostUsername { get; set; }
    public int CurrentAmountOfPlayers { get; set; }
}