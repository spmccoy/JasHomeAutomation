using System.Reactive.Concurrency;
using NetDaemon.Extensions.Scheduler;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.apps.House;

[NetDaemonApp]
public class Timer
{
    public Timer(
        LightEntities lights,
        IScheduler scheduler, 
        IHouseService houseService, 
        ILogger<Timer> logger)
    {
        scheduler.ScheduleCron("*/15 * * * *", () =>
        {
            logger.LogDebug("Determining and setting the outside lights.");
            houseService.DetermineAndSetOutsideLights();
            lights.Ratgdov25i0a070cLight.TurnOff();
        });
    }
}