using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Roborally.core.application.ApplicationContracts.GameTimer;

namespace Roborally.infrastructure.persistence.GameTimer;

public class ProgrammingTimeoutJob : IJob
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ProgrammingTimeoutJob(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var gameIdString = context.JobDetail.JobDataMap.GetString("gameId");
        var gameId = Guid.Parse(gameIdString!);

        using var scope = _scopeFactory.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<IProgrammingTimeoutHandler>();
        await handler.HandleTimeoutAsync(gameId, context.CancellationToken);
    }
}

