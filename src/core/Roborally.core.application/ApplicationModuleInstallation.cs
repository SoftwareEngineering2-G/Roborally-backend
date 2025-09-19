using FastEndpoints;

namespace Roborally.core.application;

public static class ApplicationModuleInstallation
{
    public static IServiceProvider RegisterApplicationModule(this IServiceProvider services)
    {
        var assembly = typeof(ApplicationModuleInstallation).Assembly;

        // Get all handler types that implement ICommandHandler<,>
        var handlerTypes = assembly.GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract)
            .Where(type => type.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)))
            .ToList();

        // Register each handler with its interface
        foreach (var handlerType in handlerTypes)
        {
            var handlerInterface = handlerType.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>));

            services.RegisterGenericCommand(handlerType, handlerInterface);
        }

        return services;
    }
}