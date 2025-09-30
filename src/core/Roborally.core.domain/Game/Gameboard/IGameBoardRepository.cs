namespace Roborally.core.domain.Game.Gameboard;

public interface IGameBoardRepository {
    Task<GameBoard?> FindAsync(string name, CancellationToken ct = default);
    Task AddAsync(GameBoard board, CancellationToken ct = default);
}
