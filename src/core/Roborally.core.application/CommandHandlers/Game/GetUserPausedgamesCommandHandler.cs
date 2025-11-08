using System.Collections.Immutable;
using FastEndpoints;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts;
using Roborally.core.domain.Lobby;

namespace Roborally.core.application.CommandHandlers.GameLobby;

public class GetUserPausedGamesCommandHandler : ICommandHandler<GetUserPausedGamesCommand, IList<GetUserPausedGamesResponse>> {
    private readonly IGameRepository _gameRepository;

    public GetUserPausedGamesCommandHandler(IGameRepository gameRepository) {
        _gameRepository = gameRepository;
    }

    public async Task<IList<GetUserPausedGamesResponse>> ExecuteAsync(GetUserPausedGamesCommand command,
        CancellationToken ct) {
        var games = await _gameRepository.FindPausedGamesForUserAsync(command.Username, ct);
        return games.Select(game => new GetUserPausedGamesResponse() {
            GameId = game.GameId,
            HostUsername = game.HostUsername,
            Name = game.Name,
            PlayerUsernames = game.Players.Select(x => x.Username).ToArray(),
        }).ToImmutableList();
    }
}