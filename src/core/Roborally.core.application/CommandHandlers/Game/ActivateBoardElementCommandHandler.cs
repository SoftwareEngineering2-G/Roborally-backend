using FastEndpoints;
using Roborally.core.application.CommandContracts.Game;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game;
using Roborally.core.domain.Game.Gameboard.BoardElement;

namespace Roborally.core.application.CommandHandlers.Game;

public class ActivateBoardElementCommandHandler : ICommandHandler<ActivateBoardElementCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGameRepository _gameRepository;

    public ActivateBoardElementCommandHandler(IUnitOfWork unitOfWork, IGameRepository gameRepository)
    {
        _unitOfWork = unitOfWork;
        _gameRepository = gameRepository;
    }

    public async Task ExecuteAsync(ActivateBoardElementCommand command, CancellationToken ct)
    {
        var game = await _gameRepository.FindAsync(command.GameId, ct);

        if (game is null)
            throw new CustomException("Game does not exist", 404);
        
        var player = game.Players.FirstOrDefault(p => p.Username == command.Username);
        
        if (player is null)
            throw new CustomException("Player does not exist", 404);
        
        var space = game.GameBoard.GetSpaceAt(player.CurrentPosition);

        if (space is BoardElement boardElement)
        {
            // boardElement.Activate(player);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}