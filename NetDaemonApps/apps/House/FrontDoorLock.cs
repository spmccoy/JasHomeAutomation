using Domain.Entities;
using NetDaemonApps.Interfaces;
using NetDaemonApps.Services;

namespace NetDaemonApps.apps.House.Devices;

[NetDaemonApp]
public class FrontDoorLock
{
    public FrontDoorLock(LockEntities locks, IHouseService houseService)
    {
        locks.HomeConnect620ConnectedSmartLock.StateChanges()
            .Where(w => w.New?.State == HaState.Locked)
            .Subscribe(_ =>
            {
                houseService.DetermineAndSetHouseState();
            });
        
        locks.HomeConnect620ConnectedSmartLock.StateChanges()
            .Where(w => w.New?.State == HaState.Unlocked)
            .Subscribe(_ =>
            {
                houseService.DetermineAndSetHouseState();
            });
    }
}