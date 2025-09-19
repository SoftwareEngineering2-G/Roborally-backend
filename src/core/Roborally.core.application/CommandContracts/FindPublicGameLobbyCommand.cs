using FastEndpoints;
using Roborally.core.domain.Lobby;

namespace Roborally.core.application.Contracts;

public class FindPublicGameLobbyCommand : ICommand<List<GameLobby>>
{
}