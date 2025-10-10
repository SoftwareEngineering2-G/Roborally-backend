using FastEndpoints;
using Roborally.core.application.Broadcasters;
using Roborally.core.application.CommandContracts.Game;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game;

namespace Roborally.core.application.CommandHandlers.Game;

public class DealDecksToAllCommandHandler : ICommandHandler<DealDecksToAllCommand> {
    private readonly IGameRepository _gameRepository;
    private readonly ISystemTime _systemTime;
    private readonly IIndividualPlayerBroadcaster _individualPlayerBroadcaster;
    private readonly IUnitOfWork _unitOfWork;

    public DealDecksToAllCommandHandler(IGameRepository gameRepository, ISystemTime systemTime,
        IIndividualPlayerBroadcaster individualPlayerBroadcaster, IUnitOfWork unitOfWork) {
        _gameRepository = gameRepository;
        _systemTime = systemTime;
        _individualPlayerBroadcaster = individualPlayerBroadcaster;
        _unitOfWork = unitOfWork;
    }


    public async Task ExecuteAsync(DealDecksToAllCommand command, CancellationToken ct) {
        domain.Game.Game? game = await _gameRepository.FindAsync(command.GameId, ct);
        if (game is null) {
            throw new CustomException("Game does not exist", 404);
        }

        var playerDealtCards = game.DealDecksToAllPlayers(_systemTime);

        List<Task> broadcastTasks = (from playerDealtCard in playerDealtCards
                let player = playerDealtCard.Key
                let cards = playerDealtCard.Value.Select(card => card.DisplayName).ToList()
                select _individualPlayerBroadcaster.BroadcastHandToPlayerAsync(player.Username, player.GameId, cards,
                    ct))
            .ToList();

        // Await all broadcast tasks to ensure they complete and are done in parallel
        await Task.WhenAll(broadcastTasks);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}