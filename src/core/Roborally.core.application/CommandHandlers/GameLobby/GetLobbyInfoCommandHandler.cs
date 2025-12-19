using FastEndpoints;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts;
using Roborally.core.domain;
using Roborally.core.domain.Lobby;

namespace Roborally.core.application.CommandHandlers.GameLobby;

public class GetLobbyInfoCommandHandler : ICommandHandler<GetLobbyInfoCommand, GetLobbyInfoCommandResponse> {
    private readonly IGameLobbyRepository _gameLobbyRepository;
    private readonly IGameRepository _gameRepository;

/// <author name="Truong Son NGO 2025-11-12 15:35:28 +0100 13" />
    public GetLobbyInfoCommandHandler(IGameLobbyRepository gameLobbyRepository, IGameRepository gameRepository) {
        _gameLobbyRepository = gameLobbyRepository;
        _gameRepository = gameRepository;
    }


/// <author name="Sachin Baral 2025-09-20 20:52:08 +0200 19" />
    public async Task<GetLobbyInfoCommandResponse> ExecuteAsync(GetLobbyInfoCommand command, CancellationToken ct) {
        domain.Lobby.GameLobby? lobby = await _gameLobbyRepository.FindAsync(command.GameId);

        if (lobby is null) {
            throw new CustomException("Lobby not found", 404);
        }

        List<string> joinedUserNames = lobby.JoinedUsers.Select(users => users.Username).ToList();
        bool contains = joinedUserNames.Contains(command.Username);
        if (!contains) {
            throw new CustomException("User does not have access to this lobby", 403);
        }
        
        List<string> requiredUserNames = lobby.RequiredUsers.Select(users => users.Username).ToList();
        if (lobby.RequiredUsers.Count > 0 && !requiredUserNames.Contains(command.Username)) {
            throw new CustomException("User does not have access to this lobby", 403);
        }

        string? pausedGameBoardName = null;
        if (lobby.RequiredUsers.Count > 0)
        {
            domain.Game.Game? game = await _gameRepository.FindAsync(lobby.GameId, ct);
            if (game == null) throw new CustomException("Associated game not found", 404);
            if (!game.IsPaused) throw new CustomException("Game is not paused", 400);
            pausedGameBoardName = game.GameBoard.Name;
        }

        return new GetLobbyInfoCommandResponse() {
            GameId = lobby.GameId,
            Lobbyname = lobby.Name,
            HostUsername = lobby.HostUsername,
            JoinedUsernames = joinedUserNames,
            RequiredUsernames = requiredUserNames,
            PausedGameBoardName = pausedGameBoardName
        };
    }
}