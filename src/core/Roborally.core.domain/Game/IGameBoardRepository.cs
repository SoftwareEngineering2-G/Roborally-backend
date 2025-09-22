namespace Roborally.core.domain.Game;

public interface IGameBoardRepository
{
    Task AddAsync(GameBoard gameBoard, CancellationToken cancellationToken = default);
    Task<GameBoard?> FindAsync(Guid gameBoardId, CancellationToken cancellationToken = default);
}