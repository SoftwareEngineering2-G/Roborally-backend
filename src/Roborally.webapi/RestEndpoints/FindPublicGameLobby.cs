using FastEndpoints;
using Roborally.core.application.Contracts;
using Roborally.core.domain.Lobby;

namespace Roborally.webapi.RestEndpoints;

public class FindPublicGameLobby : EndpointWithoutRequest<FindPublicGameLobbyResponse>
{
    public override void Configure()
    {
        Get("/game-lobbies");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        FindPublicGameLobbyCommand command = new FindPublicGameLobbyCommand() { };
        List<GameLobby> lobbies = await command.ExecuteAsync(ct);
        await Send.OkAsync(new FindPublicGameLobbyResponse
        {
            publicLobbies = lobbies
        }, ct);
    }
}

public class FindPublicGameLobbyResponse
{
    public List<GameLobby> publicLobbies { get; set; }
}