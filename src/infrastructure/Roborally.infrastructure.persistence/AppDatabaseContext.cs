using Microsoft.EntityFrameworkCore;
using FastEndpoints;
using Roborally.core.domain.Bases;
using Roborally.core.domain.Game;
using Roborally.core.domain.Game.Gameboard;
using Roborally.core.domain.Game.Player.Events;

namespace Roborally.infrastructure.persistence;

public class AppDatabaseContext : DbContext
{
    public AppDatabaseContext(DbContextOptions<AppDatabaseContext> options) : base(options)
    {
    }

    public required DbSet<core.domain.User.User> Users { get; set; }
    public required DbSet<core.domain.Lobby.GameLobby> GameLobby { get; set; }
    public required DbSet<PlayerEvent> PlayerEvents { get; set; }
    public required DbSet<core.domain.Game.Game> Games { get; set; }

    public required DbSet<GameBoard> GameBoards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDatabaseContext).Assembly);
    }
}