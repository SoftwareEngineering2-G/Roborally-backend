using FastEndpoints;
using Roborally.core.application.Contracts;

namespace Roborally.webapi.RestEndpoints;

public class CreateGameLobby : Endpoint<CreateGameLobbyRequest, CreateGameLobbyResponse>
{
    public override void Configure()
    {
        Post("/game-lobbies");
    }

    public override async Task HandleAsync(CreateGameLobbyRequest req, CancellationToken ct)
    {
        CreateGameLobbyCommand command = new CreateGameLobbyCommand()
        {
            GameRoomName = req.GameRoomName,
            IsPrivate = req.IsPrivate,
            HostUserId = req.HostUserId,
        };
        Guid gameid = await command.ExecuteAsync(ct);
        await Send.OkAsync(new CreateGameLobbyResponse
        {
            GameRoomId = gameid
        }, ct);
    }
}

public class CreateGameLobbyRequest
{
    public Guid HostUserId { get; set; }
    public required string GameRoomName { get; set; }
    public bool IsPrivate { get; set; }
}

public class CreateGameLobbyResponse
{
    public Guid GameRoomId { get; set; }
}