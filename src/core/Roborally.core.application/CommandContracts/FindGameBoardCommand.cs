using FastEndpoints;
using Roborally.core.domain.Game;

namespace Roborally.core.application.CommandContracts;

public class FindGameBoardCommand:ICommand<GameBoard>
{
    public required Guid Id { get; set; }
}