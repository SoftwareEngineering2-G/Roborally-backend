using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Roborally.core.application.ApplicationContracts.GameTimer;

namespace Roborally.infrastructure.persistence.GameTimer;

public class GameTimerService : IGameTimerService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ConcurrentDictionary<Guid, CancellationTokenSource> _timers = new();

    public GameTimerService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public void StartProgrammingTimer(Guid gameId, TimeSpan duration)
    {
        CancelProgrammingTimer(gameId);

        var cts = new CancellationTokenSource();
        _timers[gameId] = cts;
        
        _ = Task.Run(async () =>
        {
            await Task.Delay(duration, cts.Token);

            if (!cts.Token.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<IProgrammingTimeoutHandler>();
                await handler.HandleTimeoutAsync(gameId, cts.Token);
                _timers.TryRemove(gameId, out _);
            }
        }, cts.Token);
    }

    public void CancelProgrammingTimer(Guid gameId)
    {
        if (!_timers.TryRemove(gameId, out var cts)) return;
        
        cts.Cancel();
        cts.Dispose();
    }
}