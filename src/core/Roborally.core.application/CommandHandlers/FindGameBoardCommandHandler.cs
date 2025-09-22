using FastEndpoints;
using Roborally.core.application.CommandContracts;
using Roborally.core.domain.Game;

namespace Roborally.core.application.CommandHandlers;

public class FindGameBoardCommandHandler:ICommandHandler<FindGameBoardCommand, GameBoard>
{
    public readonly IGameBoardRepository _gameBoardRepository;
    
    public FindGameBoardCommandHandler(IGameBoardRepository gameBoardRepository)
    {
        _gameBoardRepository = gameBoardRepository;
    }
    
    public async Task<GameBoard> ExecuteAsync(FindGameBoardCommand command, CancellationToken ct)
    {
        var board =  await _gameBoardRepository.FindAsync(command.Id, ct);
        if (board == null)
        {
            throw new Exception("Game board not found");
        }
        return board;
    }
}
