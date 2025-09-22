using FastEndpoints;
using Roborally.core.application.CommandContracts;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game;

namespace Roborally.core.application.CommandHandlers;

public class CreateGameCommandHandler: ICommandHandler<CreateGameCommand, string>
{
    private readonly IGameRepository _gameRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IGameBoardRepository _gameBoardRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateGameCommandHandler(IGameRepository gameRepository, IPlayerRepository playerRepository,
        IGameBoardRepository gameBoardRepository, IUnitOfWork unitOfWork)
    {
        _gameRepository = gameRepository;
        _playerRepository = playerRepository;
        _gameBoardRepository = gameBoardRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<string> ExecuteAsync(CreateGameCommand command, CancellationToken ct)
    {
        var game = new Game
        {
            GameBoard = await _gameBoardRepository.FindAsync(command.GameBoardId, ct) 
                        ?? throw new Exception("Game board not found"),
            GamePhase = GamePhase.ProgrammingPhase
        }; 
        
        
        foreach (var playerId in command.PlayerIds)
        {
            var player = await _playerRepository.FindAsync(playerId, ct) 
                         ?? throw new Exception($"Player with ID {playerId} not found");
            game.AddPlayer(player);
        }
        await _gameRepository.AddAsync(game, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return game.Id.ToString();
    }
}