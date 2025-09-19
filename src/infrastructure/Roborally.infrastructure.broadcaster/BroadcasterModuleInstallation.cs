using Microsoft.Extensions.DependencyInjection;
using Roborally.core.application;

namespace Roborally.infrastructure.broadcaster;

public static class BroadcasterModuleInstallation
{
    public static IServiceCollection InstallBroadcasterModule(this IServiceCollection services)
    {
        services.AddSignalR();
        services.AddScoped<IGameLobbyBroadcaster, GameLobbyBroadcaster>();
        
        return services;
    }
}
