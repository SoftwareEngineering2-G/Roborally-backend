using FastEndpoints;
using Roborally.core.application.CommandContracts;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game;

namespace Roborally.core.application.CommandHandlers;

public class CreateGameBoardCommandHandler: ICommandHandler<CreateGameBoardCommand, Guid>
{
    private readonly IGameBoardRepository _gameBoardRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateGameBoardCommandHandler( IGameBoardRepository gameBoardRepository, IUnitOfWork unitOfWork)
    {
        _gameBoardRepository = gameBoardRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Guid> ExecuteAsync(CreateGameBoardCommand command, CancellationToken ct)
    {
        var gameBoard = new GameBoard()
        {
            SpaceNames = command.BoardSpaces
        };
        await _gameBoardRepository.AddAsync(gameBoard, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return gameBoard.Id;
    }
}