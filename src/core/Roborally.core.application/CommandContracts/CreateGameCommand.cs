using System.Windows.Input;
using FastEndpoints;

namespace Roborally.core.application.CommandContracts;

public class CreateGameCommand : ICommand<string>
{
    public IList<Guid> PlayerIds { get; set; }
    public Guid GameBoardId { get; set; }
}
