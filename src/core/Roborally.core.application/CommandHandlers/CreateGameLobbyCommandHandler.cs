using FastEndpoints;
using Roborally.core.application.CommandContracts;
using Roborally.core.domain;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Lobby;
using Roborally.core.domain.User;

namespace Roborally.core.application.CommandHandlers;

public class CreateGameLobbyCommandHandler : ICommandHandler<CreateGameLobbyCommand, Guid>
{
    private readonly IGameLobbyRepository  _gameLobbyRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISystemTime _systemTime;

    public CreateGameLobbyCommandHandler(IGameLobbyRepository gameLobbyRepository, IUserRepository userRepository, IUnitOfWork unitOfWork, ISystemTime systemTime)
    {
        _gameLobbyRepository = gameLobbyRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _systemTime = systemTime;
    }

    public async Task<Guid> ExecuteAsync(CreateGameLobbyCommand command, CancellationToken ct)
    {
        var hostUser = await _userRepository.FindAsync(command.HostUsername, ct);
        if (hostUser == null)
            throw new CustomException("Host user not found", 409);
        
        var isCurrentlyHosting = await _gameLobbyRepository.IsUserCurrentlyHostingActiveLobbyAsync(command.HostUsername);
        if (isCurrentlyHosting)
        {
            throw new CustomException("User is already hosting an active lobby", 409);
        }

        var lobby = new GameLobby(hostUser, command.IsPrivate, command.GameRoomName, _systemTime);
        
        await _gameLobbyRepository.AddAsync(lobby, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return lobby.GameId;

    }
}