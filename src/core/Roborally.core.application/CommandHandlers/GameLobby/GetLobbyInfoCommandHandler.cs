using FastEndpoints;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts;
using Roborally.core.domain;
using Roborally.core.domain.Lobby;

namespace Roborally.core.application.CommandHandlers.GameLobby;

public class GetLobbyInfoCommandHandler : ICommandHandler<GetLobbyInfoCommand, GetLobbyInfoCommandResponse> {
    private readonly IGameLobbyRepository _gameLobbyRepository;

    public GetLobbyInfoCommandHandler(IGameLobbyRepository gameLobbyRepository) {
        _gameLobbyRepository = gameLobbyRepository;
    }


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

        return new GetLobbyInfoCommandResponse() {
            GameId = lobby.GameId,
            Lobbyname = lobby.Name,
            HostUsername = lobby.HostUsername,
            JoinedUsernames = joinedUserNames,
            RequiredUsernames = requiredUserNames
        };
    }
}