using FastEndpoints;

namespace Roborally.core.application.CommandContracts;

public class GetGamesByPlayerCommand:ICommand<List<Roborally.core.domain.Game.Game>>
{
    public Guid PlayerId { get; set; }
}