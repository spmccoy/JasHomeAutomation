using NetDaemonApps.Services;

namespace NetDaemonApps.apps.House.Devices;

[NetDaemonApp]
public class Shawn
{
    private readonly Entities _entities;
    private readonly IHouseService _houseService;

    public Shawn(Entities entities, IHouseService houseService)
    {
        _entities = entities;
        _houseService = houseService;
        
        entities.Person.Shawn.StateChanges()
            .Where(w => w.New?.State == HaState.Away)
            .Subscribe(_ => HandleAway());
        
        entities.Person.Shawn.StateChanges()
            .Where(w => w.New?.State == HaState.Home)
            .Subscribe(_ => HandleHome());
    }

    private void HandleAway()
    {
        _entities.Switch.ShawnroomMainNetdaemon.TurnOff();
        _entities.Cover.Ratgdov25i0a070cDoor.CloseCover();
    }

    private void HandleHome()
    {
        _entities.Cover.Ratgdov25i0a070cDoor.OpenCover();
    }
}