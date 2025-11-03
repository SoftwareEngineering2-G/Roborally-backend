using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.domain.Bases;
using Roborally.infrastructure.persistence.Contracts;
using Roborally.infrastructure.persistence.Game;
using Roborally.infrastructure.persistence.Lobby;
using Roborally.infrastructure.persistence.User;

namespace Roborally.infrastructure.persistence;

public static class PersistenceModuleInstallation
{
    public static IServiceCollection InstallPersistenceModule(this IServiceCollection services,
        string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));
        }

        services.AddDbContext<AppDatabaseContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IGameLobbyRepository, GameLobbyRepository>();
        services.AddScoped<IGameRepository, GameRepository>();
        services.AddScoped<IGameBoardRepository, GameBoardRepository>();
        services.AddScoped<ISystemTime, SystemTime>();
        services.AddScoped<GameBoardSeeder>();
        return services;
    }
}