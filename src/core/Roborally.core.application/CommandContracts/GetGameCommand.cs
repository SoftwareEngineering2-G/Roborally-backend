using FastEndpoints;
using Roborally.core.domain.Game;

namespace Roborally.core.application.CommandContracts;

public class GetGameCommand:ICommand<Game>
{
    public Guid GameId { get; set; }
}