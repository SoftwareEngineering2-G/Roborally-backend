using FastEndpoints;
using Roborally.core.application.Contracts;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Lobby;
using Roborally.core.domain.User;

namespace Roborally.core.application.Handlers;

public class CreateGameLobbyCommandHandler : ICommandHandler<CreateGameLobbyCommand, Guid>
{
    private readonly IGameLobbyRepository  _gameLobbyRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateGameLobbyCommandHandler(IGameLobbyRepository gameLobbyRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _gameLobbyRepository = gameLobbyRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> ExecuteAsync(CreateGameLobbyCommand command, CancellationToken ct)
    {
        var hostUser = await _userRepository.FindAsync(command.HostUserId);
        if (hostUser == null)
            throw new CustomException("Host user not found", 409);

        var lobby = new GameLobby(hostUser, command.IsPrivate, command.GameRoomName)
        {
            GameId = Guid.CreateVersion7(),
            IsPrivate = false
        };
        
        await _gameLobbyRepository.AddAsync(lobby, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return lobby.GameId;

    }
}