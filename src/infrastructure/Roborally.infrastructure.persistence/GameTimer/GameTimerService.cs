using Quartz;
using Roborally.core.application.ApplicationContracts.GameTimer;

namespace Roborally.infrastructure.persistence.GameTimer;

public class GameTimerService : IGameTimerService
{
    private readonly ISchedulerFactory _schedulerFactory;

/// <author name="Vincenzo Altaserse 2025-12-18 17:40:31 +0100 10" />
    public GameTimerService(ISchedulerFactory schedulerFactory)
    {
        _schedulerFactory = schedulerFactory;
    }

/// <author name="Vincenzo Altaserse 2025-12-18 17:40:31 +0100 15" />
    public void StartProgrammingTimer(Guid gameId, TimeSpan duration)
    {
        Task.Run(async () =>
        {
            var scheduler = await _schedulerFactory.GetScheduler();

            // Cancel any existing timer for this game
            var existingJobKey = new JobKey($"ProgrammingTimeout-{gameId}");
            if (await scheduler.CheckExists(existingJobKey))
            {
                await scheduler.DeleteJob(existingJobKey);
            }

            // Create the job
            var job = JobBuilder.Create<ProgrammingTimeoutJob>()
                .WithIdentity(existingJobKey)
                .UsingJobData("gameId", gameId.ToString())
                .Build();

            // Create the trigger
            var trigger = TriggerBuilder.Create()
                .WithIdentity($"ProgrammingTimeoutTrigger-{gameId}")
                .StartAt(DateTimeOffset.UtcNow.Add(duration))
                .Build();

            // Schedule the job
            await scheduler.ScheduleJob(job, trigger);
        });
    }

/// <author name="Vincenzo Altaserse 2025-12-18 17:40:31 +0100 45" />
    public void CancelProgrammingTimer(Guid gameId)
    {
        Task.Run(async () =>
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            var jobKey = new JobKey($"ProgrammingTimeout-{gameId}");
            await scheduler.DeleteJob(jobKey);
        });
    }
}