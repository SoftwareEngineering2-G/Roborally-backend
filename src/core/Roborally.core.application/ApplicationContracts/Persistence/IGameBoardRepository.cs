using Roborally.core.domain.Game.Gameboard;

namespace Roborally.core.application.ApplicationContracts.Persistence;

public interface IGameBoardRepository {
    Task<GameBoard?> FindAsync(string name, CancellationToken ct = default);
    Task<List<GameBoard>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(GameBoard board, CancellationToken ct = default);
}