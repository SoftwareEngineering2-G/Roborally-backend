using Quartz;
using Roborally.core.application.ApplicationContracts.GameTimer;

namespace Roborally.infrastructure.persistence.GameTimer;

public class GameTimerService : IGameTimerService
{
    private readonly ISchedulerFactory _schedulerFactory;

    public GameTimerService(ISchedulerFactory schedulerFactory)
    {
        _schedulerFactory = schedulerFactory;
    }

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