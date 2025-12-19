using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Roborally.core.application.ApplicationContracts.GameTimer;

namespace Roborally.infrastructure.persistence.GameTimer;

public class ProgrammingTimeoutJob : IJob
{
    private readonly IServiceScopeFactory _scopeFactory;

/// <author name="Vincenzo Altaserse 2025-12-18 17:40:31 +0100 11" />
    public ProgrammingTimeoutJob(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

/// <author name="Vincenzo Altaserse 2025-12-18 17:40:31 +0100 16" />
    public async Task Execute(IJobExecutionContext context)
    {
        var gameIdString = context.JobDetail.JobDataMap.GetString("gameId");
        var gameId = Guid.Parse(gameIdString!);

        using var scope = _scopeFactory.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<IProgrammingTimeoutHandler>();
        await handler.HandleTimeoutAsync(gameId, context.CancellationToken);
    }
}
