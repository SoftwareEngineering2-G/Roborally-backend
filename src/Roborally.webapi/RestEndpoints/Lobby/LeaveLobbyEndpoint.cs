﻿using FastEndpoints;
using Roborally.core.application.CommandContracts;

namespace Roborally.webapi.RestEndpoints.Lobby;

public class LeaveLobbyEndpoint : Endpoint<LeaveLobbyRequest>
{
    public override void Configure()
    {
        Post("/game-lobbies/{gameId}/leave");
        Summary(s => {
            s.Summary = "Leave a game lobby";
            s.Description = "Allows a user to leave an existing game lobby";
            s.Response(200, "Successfully left the lobby");
            s.Response(404, "User or lobby not found");
        });
    }
    
    public override async Task HandleAsync(LeaveLobbyRequest req, CancellationToken ct)
    {
        LeaveLobbyCommand command = new LeaveLobbyCommand()
        {
            Username = req.Username,
            GameId = req.GameId
        };

        await command.ExecuteAsync(ct);
        await Send.OkAsync(cancellation: ct);
    }
}

public class LeaveLobbyRequest
{
    public Guid GameId { get; set; }
    public string Username { get; set; }
}