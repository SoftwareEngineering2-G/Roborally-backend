using FastEndpoints;
using Roborally.core.application.CommandContracts;
using Roborally.core.domain.Game;
using Roborally.core.domain.User;

namespace Roborally.core.application.CommandHandlers;

public class GetGameCommandHandler:ICommandHandler<GetGameCommand, Game>
{
    private readonly IGameRepository _gameRepository;
    
    public GetGameCommandHandler(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }
    public async Task<Game> ExecuteAsync(GetGameCommand command, CancellationToken ct)
    {
        var game = await _gameRepository.FindAsync(command.GameId, ct) 
                   ?? throw new Exception("Game not found");
        return game;
    }
}