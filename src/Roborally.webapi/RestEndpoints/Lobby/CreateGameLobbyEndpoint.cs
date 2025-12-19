using FastEndpoints;
using Roborally.core.application.CommandContracts;

namespace Roborally.webapi.RestEndpoints.Lobby;

public class CreateGameLobbyEndpoint : Endpoint<CreateGameLobbyRequest, CreateGameLobbyResponse>
{
/// <author name="Suhani Pandey 2025-09-17 13:49:31 +0200 8" />
    public override void Configure()
    {
        Post("/game-lobbies");
    }

/// <author name="Suhani Pandey 2025-09-17 13:49:31 +0200 13" />
    public override async Task HandleAsync(CreateGameLobbyRequest req, CancellationToken ct)
    {
        CreateGameLobbyCommand command = new CreateGameLobbyCommand()
        {
            GameRoomName = req.GameRoomName,
            IsPrivate = req.IsPrivate,
            HostUsername = req.HostUsername,
        };
        Guid gameId = await command.ExecuteAsync(ct);
        await Send.OkAsync(new CreateGameLobbyResponse
        {
            GameRoomId = gameId
        }, ct);
    }
}

public class CreateGameLobbyRequest
{
    public string HostUsername { get; set; }
    public required string GameRoomName { get; set; }
    public bool IsPrivate { get; set; }
}

public class CreateGameLobbyResponse
{
    public Guid GameRoomId { get; set; }
}