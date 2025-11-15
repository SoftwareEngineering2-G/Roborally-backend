using FastEndpoints;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.domain.Bases;
using Roborally.core.domain;

namespace Roborally.webapi.RestEndpoints.Test;

// This endpoint is made for testing purposes, therefore doesnt follow our standard approach...
public class CompleteGameEndpoint : Endpoint<CompleteGameRequest>
{
    private readonly IGameRepository _gameRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISystemTime _systemTime;

    public CompleteGameEndpoint(IGameRepository gameRepository, IUnitOfWork unitOfWork, ISystemTime systemTime)
    {
        _gameRepository = gameRepository;
        _unitOfWork = unitOfWork;
        _systemTime = systemTime;
    }

    public override void Configure()
    {
        Post("/test/complete-game");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CompleteGameRequest req, CancellationToken ct)
    {
        var game = await _gameRepository.FindAsync(req.GameId, ct);
        if (game is null)
            throw new CustomException("Game not found", 404);

        // Choose a player to mark as winner for testing. Use the first player in the game's player list.
        var player = game.Players.FirstOrDefault();
        if (player is null)
            throw new CustomException("Game has no players", 400);

        // Force completion
        await game.HandleGameCompleted(player, _systemTime);

        // Persist rating changes / completedAt etc.
        await _unitOfWork.SaveChangesAsync(ct);

        await Send.OkAsync(new { Message = "Game completed (test) invoked", GameId = req.GameId }, cancellation: ct);
    }
}

public class CompleteGameRequest
{
    public Guid GameId { get; set; }
}

