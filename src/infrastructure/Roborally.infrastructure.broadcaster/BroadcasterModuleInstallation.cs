using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Roborally.core.application;
using Roborally.core.application.ApplicationContracts.Broadcasters;
using Roborally.infrastructure.broadcaster.Broadcasters;
using Roborally.infrastructure.broadcaster.Game;
using Roborally.infrastructure.broadcaster.GameLobby;

namespace Roborally.infrastructure.broadcaster;

public static class BroadcasterModuleInstallation
{
    public static IServiceCollection InstallBroadcasterModule(this IServiceCollection services)
    {
        services.AddSignalR();
        services.AddScoped<IGameLobbyBroadcaster, GameLobbyBroadcaster>();
        services.AddScoped<IGameBroadcaster, GameBroadcaster>();
        services.AddScoped<IIndividualPlayerBroadcaster, IndividualPlayerBroadcaster>();
        
        return services;
    }

    public static WebApplication InstallBroadcasterModule(this WebApplication application)
    {
        // This method can be used to resolve services if needed
        application.MapHub<GameLobbyHub>("/game-lobbies");
        application.MapHub<GameHub>("/game");


        return application;
    }
}
