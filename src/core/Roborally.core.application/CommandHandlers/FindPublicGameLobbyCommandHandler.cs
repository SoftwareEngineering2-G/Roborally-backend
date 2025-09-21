using System.Collections.Immutable;
using FastEndpoints;
using Roborally.core.application.CommandContracts;
using Roborally.core.domain.Lobby;

namespace Roborally.core.application.CommandHandlers;

public class FindPublicGameLobbyCommandHandler : ICommandHandler<GetActiveGameLobbiesCommand, IList<GetActiveGameLobbyCommandResponse>> {
    private readonly IGameLobbyRepository _gameLobbyRepository;

    public FindPublicGameLobbyCommandHandler(IGameLobbyRepository gameLobbyRepository) {
        _gameLobbyRepository = gameLobbyRepository;
    }

    public async Task<IList<GetActiveGameLobbyCommandResponse>> ExecuteAsync(GetActiveGameLobbiesCommand command,
        CancellationToken ct) {
        var gameLobbies = await _gameLobbyRepository.FindPublicLobbiesAsync(ct);
        return gameLobbies.Select(gl => new GetActiveGameLobbyCommandResponse() {
            CurrentAmountOfPlayers = gl.JoinedUsers.Count,
            GameId = gl.GameId,
            HostUsername = gl.HostUsername,
            Name = gl.Name
        }).ToImmutableList();
    }
}