using FastEndpoints;
using Roborally.core.application.Contracts;
using Roborally.core.domain.Lobby;

namespace Roborally.core.application.Handlers;

public class FindPublicGameLobbyCommandHandler : ICommandHandler<FindPublicGameLobbyCommand, List<GameLobby>>
{
    private readonly IGameLobbyRepository _gameLobbyRepository;

    public FindPublicGameLobbyCommandHandler(IGameLobbyRepository gameLobbyRepository)
    {
        _gameLobbyRepository = gameLobbyRepository;
    }

    public async Task<List<GameLobby>> ExecuteAsync(FindPublicGameLobbyCommand command, CancellationToken ct)
    {
        var gameLobbies = await _gameLobbyRepository.FindPublicLobbiesAsync(ct);
        return gameLobbies;
    }
}