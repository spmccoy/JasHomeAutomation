using System.Reactive.Concurrency;
using NetDaemon.Extensions.Scheduler;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.apps.House;

[NetDaemonApp]
public class Poller
{
    public Poller(IScheduler scheduler, IHouseService houseService, ILogger<Poller> logger)
    {
        scheduler.ScheduleCron("*/15 * * * *", () =>
        {
            logger.LogInformation("Determining and setting the outside lights.");
            houseService.DetermineAndSetOutsideLights();
        });
    }
}