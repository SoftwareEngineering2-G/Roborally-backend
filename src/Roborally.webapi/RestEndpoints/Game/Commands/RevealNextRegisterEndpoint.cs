using FastEndpoints;
using Roborally.core.application.CommandContracts.Game;

namespace Roborally.webapi.RestEndpoints.Game.Commands;

public class RevealNextRegisterEndpoint : Endpoint<RevealNextRegisterRequest, RevealNextRegisterResponse>
{
    public override void Configure()
    {
        Post("/games/{gameId}/reveal-next-register");
    }

    public override async Task HandleAsync(RevealNextRegisterRequest req, CancellationToken ct)
    {
        RevealNextRegisterCommand command = new RevealNextRegisterCommand()
        {
            GameId = req.GameId,
            Username = req.Username
        };

        var result = await command.ExecuteAsync(ct);
        
        await Send.OkAsync(new RevealNextRegisterResponse
        {
            RegisterNumber = result.RegisterNumber,
            RevealedCards = result.RevealedCards.Select(c => new RevealNextRegisterResponse.RevealedCard
            {
                Username = c.Username,
                Card = c.Card
            }).ToList()
        }, ct);
    }
}

public class RevealNextRegisterRequest
{
    public required string Username { get; init; }
    public Guid GameId { get; init; }
}

public class RevealNextRegisterResponse
{
    public required int RegisterNumber { get; init; }
    public required List<RevealedCard> RevealedCards { get; init; }

    public class RevealedCard
    {
        public required string Username { get; init; }
        public required string Card { get; init; }
    }
}
