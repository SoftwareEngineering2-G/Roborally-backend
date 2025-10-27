using FastEndpoints;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Lobby;
using Roborally.core.domain.User;

namespace Roborally.core.application.CommandHandlers.GameLobby;

public class LeaveLobbyCommandHandler : ICommandHandler<LeaveLobbyCommand>
{
    private readonly IGameLobbyRepository _gameLobbyRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGameLobbyBroadcaster _gameLobbyBroadcaster;

    public LeaveLobbyCommandHandler(IGameLobbyRepository gameLobbyRepository, IUserRepository userRepository,
        IUnitOfWork unitOfWork, IGameLobbyBroadcaster gameLobbyBroadcaster)
    {
        _gameLobbyRepository = gameLobbyRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _gameLobbyBroadcaster = gameLobbyBroadcaster;
    }

    public async Task ExecuteAsync(LeaveLobbyCommand command, CancellationToken ct)
    {
        var user = await _userRepository.FindAsync(command.Username, ct);
        if (user is null)
            throw new CustomException("User not found", 404);
        
        var gameLobby = await _gameLobbyRepository.FindAsync(command.GameId);
        if (gameLobby is null)
            throw new CustomException("Game lobby not found", 404);
        
        var currentHost = gameLobby.HostUsername;
            
        gameLobby.LeaveLobby(user);
        
        if (gameLobby.JoinedUsers.Count == 0) 
            _gameLobbyRepository.Remove(gameLobby);
        else if (gameLobby.HostUsername != currentHost)
            await _gameLobbyBroadcaster.BroadcastHostChangedAsync(command.GameId, gameLobby.HostUsername, ct);
            
        
        await _unitOfWork.SaveChangesAsync(ct);
        await _gameLobbyBroadcaster.BroadcastUserLeftAsync(command.GameId, command.Username, ct);
    }
}