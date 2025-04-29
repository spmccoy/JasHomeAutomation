using System.Reactive.Concurrency;
using NetDaemon.Extensions.Scheduler;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.apps.House;

[NetDaemonApp]
public class Poller
{
    public Poller(IScheduler scheduler, IHouseService houseService)
    {
        scheduler.ScheduleCron("*/15 * * * *", houseService.DetermineAndSetOutsideLights);
    }
}