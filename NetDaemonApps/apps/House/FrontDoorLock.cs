using NetDaemonApps.Services;

namespace NetDaemonApps.apps.House.Devices;

[NetDaemonApp]
public class FrontDoorLock
{
    public FrontDoorLock(Entities entities, IHouseService houseService)
    {
        entities.Lock.HomeConnect620ConnectedSmartLock.StateChanges()
            .Where(w => w.New?.State == HaState.Locked)
            .Subscribe(_ =>
            {
                houseService.DetermineAndSetHouseState();
            });
        
        entities.Lock.HomeConnect620ConnectedSmartLock.StateChanges()
            .Where(w => w.New?.State == HaState.Unlocked)
            .Subscribe(_ =>
            {
                houseService.DetermineAndSetHouseState();
            });
    }
}