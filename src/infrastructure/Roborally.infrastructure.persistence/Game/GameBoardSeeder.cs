using Microsoft.Extensions.Logging;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game.Gameboard;

namespace Roborally.infrastructure.persistence.Game;

public class GameBoardSeeder
{
    private readonly IGameBoardRepository _gameBoardRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GameBoardSeeder> _logger;

/// <author name="Satish Gurung 2025-11-03 14:12:46 +0100 14" />
    private static readonly Dictionary<string, string> BoardFiles = new()
    {
        { "Starter Course", "StarterCourse.json" },
        { "Castle Tour", "CastleTour.json" }
    };

    public GameBoardSeeder(
        IGameBoardRepository gameBoardRepository,
        IUnitOfWork unitOfWork,
        ILogger<GameBoardSeeder> logger)
    {
        _gameBoardRepository = gameBoardRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

/// <author name="Satish Gurung 2025-11-03 14:12:46 +0100 30" />
    public async Task SeedBoardsAsync(CancellationToken ct = default)
    {
        _logger.LogInformation("Starting game board seeding...");

        // Try Docker path first, then local development path
        string mapsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "maps");
        
        if (!Directory.Exists(mapsFolder))
        {
            // Local development path
            mapsFolder = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "..", "..", "..", "..", "..", // Navigate up from bin/Debug/net9.0 in webapi
                "src", "core", "Roborally.core.domain", "Game", "Gameboard", "maps"
            );
            mapsFolder = Path.GetFullPath(mapsFolder);
        }

        if (!Directory.Exists(mapsFolder))
        {
            _logger.LogWarning("Maps folder not found. Tried paths: Docker=/app/maps, Local=../../maps. Skipping board seeding.");
            return;
        }

        _logger.LogInformation("Maps folder found at: {MapsFolder}", mapsFolder);

        int seededCount = 0;
        int skippedCount = 0;

        foreach (var (boardName, fileName) in BoardFiles)
        {
            try
            {
                // Check if board already exists
                var existingBoard = await _gameBoardRepository.FindAsync(boardName, ct);
                if (existingBoard != null)
                {
                    _logger.LogInformation("Board '{BoardName}' already exists in database. Skipping.", boardName);
                    skippedCount++;
                    continue;
                }

                // Load board from JSON
                string filePath = Path.Combine(mapsFolder, fileName);
                if (!File.Exists(filePath))
                {
                    _logger.LogWarning("Board file not found: {FilePath}. Skipping '{BoardName}'.", filePath, boardName);
                    continue;
                }

                _logger.LogInformation("Loading board '{BoardName}' from {FilePath}...", boardName, fileName);
                var gameBoard = GameBoardJsonLoader.LoadFromJson(filePath, boardName);

                // Save to database
                await _gameBoardRepository.AddAsync(gameBoard, ct);
                await _unitOfWork.SaveChangesAsync(ct);

                _logger.LogInformation("Successfully seeded board '{BoardName}'.", boardName);
                seededCount++;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to seed board '{BoardName}' from file '{FileName}'.", boardName, fileName);
            }
        }

        _logger.LogInformation(
            "Game board seeding completed. Seeded: {SeededCount}, Skipped: {SkippedCount}",
            seededCount,
            skippedCount
        );
    }
}