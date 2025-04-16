using Domain.Entities;
using NetDaemonApps.Interfaces;
using NetDaemonApps.Services;

namespace NetDaemonApps.apps.House.Devices;

[NetDaemonApp]
public class Shawn
{
    public Shawn(Entities entities, IShawnRoomService shawnRoomService, CoverEntities covers)
    {
        entities.Person.Shawn.StateChanges()
            .Where(w => w.New?.State == HaState.Away)
            .Subscribe(_ =>
            {
                entities.Switch.ShawnroomStateNetdaemon.TurnOff();
                covers.HouseGarageDoorNetdaemon.CloseCover();
            });
        
        entities.Person.Shawn.StateChanges()
            .Where(w => w.New?.State == HaState.Home)
            .Subscribe(_ =>
            {
                covers.HouseGarageDoorNetdaemon.OpenCover();
            });
    }
}