using FastEndpoints;

namespace Roborally.core.application.CommandContracts;

public class CreateGameBoardCommand:ICommand<Guid>
{
    public required string[][] BoardSpaces { get; set; }
}