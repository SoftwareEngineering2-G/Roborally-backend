using FastEndpoints;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.application.CommandContracts;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Lobby;
using Roborally.core.domain.User;

namespace Roborally.core.application.CommandHandlers.GameLobby;

public class CreateGameLobbyCommandHandler : ICommandHandler<CreateGameLobbyCommand, Guid>
{
    private readonly IGameLobbyRepository  _gameLobbyRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISystemTime _systemTime;

/// <author name="Suhani Pandey 2025-09-17 13:49:31 +0200 18" />
    public CreateGameLobbyCommandHandler(IGameLobbyRepository gameLobbyRepository, IUserRepository userRepository, IUnitOfWork unitOfWork, ISystemTime systemTime)
    {
        _gameLobbyRepository = gameLobbyRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _systemTime = systemTime;
    }

/// <author name="Suhani Pandey 2025-09-17 13:49:31 +0200 26" />
    public async Task<Guid> ExecuteAsync(CreateGameLobbyCommand command, CancellationToken ct)
    {
        var hostUser = await _userRepository.FindAsync(command.HostUsername, ct);
        if (hostUser is null)
            throw new CustomException("Host user not found", 409);
        
        bool isCurrentlyHosting = await _gameLobbyRepository.IsUserCurrentlyHostingActiveLobbyAsync(command.HostUsername);
        if (isCurrentlyHosting)
        {
            throw new CustomException("User is already hosting an active lobby", 409);
        }

        var lobby = new domain.Lobby.GameLobby(hostUser, command.IsPrivate, command.GameRoomName, _systemTime);
        
        await _gameLobbyRepository.AddAsync(lobby, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return lobby.GameId;

    }
}