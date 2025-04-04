using System.Threading;
using NetDaemonApps.Services;

namespace NetDaemonApps.apps.House.Devices;

[NetDaemonApp]
public class Garage
{
    public Garage(Entities entities, IHouseService houseService)
    {
        entities.Cover.Ratgdov25i0a070cDoor.StateChanges()
            .Where(w => w.New?.State == HaState.Open)
            .Subscribe(_ =>
            {
                houseService.DetermineAndSetHouseState();
            });
        
        entities.Cover.Ratgdov25i0a070cDoor.StateChanges()
            .Where(w => w.New?.State == HaState.Closed)
            .Subscribe(_ =>
            {
                houseService.DetermineAndSetHouseState();
            });
    }
}