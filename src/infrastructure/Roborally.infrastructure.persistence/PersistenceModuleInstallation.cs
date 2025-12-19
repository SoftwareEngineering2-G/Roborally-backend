using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Roborally.core.application.ApplicationContracts;
using Roborally.core.application.ApplicationContracts.GameTimer;
using Roborally.core.application.ApplicationContracts.Persistence;
using Roborally.core.domain.Bases;
using Roborally.infrastructure.persistence.Authentication;
using Roborally.infrastructure.persistence.Game;
using Roborally.infrastructure.persistence.GameTimer;
using Roborally.infrastructure.persistence.Lobby;
using Roborally.infrastructure.persistence.User;

namespace Roborally.infrastructure.persistence;

public static class PersistenceModuleInstallation
{
    public static IServiceCollection InstallPersistenceModule(this IServiceCollection services,
        string connectionString, IConfiguration configuration)
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
        services.AddScoped<ISystemTime, Contracts.SystemTime>();
        services.AddScoped<GameBoardSeeder>();
        
        // Register JWT settings from configuration
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        
        // Register JWT service
        services.AddScoped<IJwtService, JwtService>();

        // Configure Quartz
        services.AddQuartz(q =>
        {
            // Use a simple in-memory job store (can be configured for persistence later if needed)
            q.UseInMemoryStore();

            // Register the job
            q.AddJob<ProgrammingTimeoutJob>(opts => opts.WithIdentity("ProgrammingTimeoutJob").StoreDurably());
        });

        // Add the Quartz.NET hosted service
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        services.AddSingleton<IGameTimerService, GameTimerService>();
        
        return services;
    }
}