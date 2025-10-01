using FastEndpoints;

namespace Roborally.core.application.CommandContracts;

public class GetGameCommand : ICommand<GetGameCommandResponse>
{
    public required Guid GameId { get; init; }
}


public class GetGameCommandResponse
{
    public core.domain.Game.Game Game { get; set; }
}