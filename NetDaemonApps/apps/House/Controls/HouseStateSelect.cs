using NetDaemonApps.Interfaces;
using NetDaemonApps.Services;

namespace NetDaemonApps.apps.House.Controls;

[NetDaemonApp]
public class HouseStateSelect : MqttSelect
{
    private readonly Entities _entities;
    private readonly IHouseService _houseService;


    public HouseStateSelect(Entities entities, IHouseService houseService) 
        : base("House", "state", "House State")
    {
        _entities = entities;
        _houseService = houseService;

        AddOption(RoomState.HomeSecure, HandleHomeSecure);
        AddOption(RoomState.HomeUnsecured, HandleHomeUnsecured);
        AddOption(RoomState.Away, HandleAway);
        AddOption(RoomState.Sleep, HandleSleep);
    }

    private void HandleHomeSecure()
    {
        _entities.Cover.Ratgdov25i0a070cDoor.CloseCover();
        _entities.Lock.HomeConnect620ConnectedSmartLock.Lock();
        _houseService.DetermineAndSetOutsideLights();
    }
    
    private void HandleHomeUnsecured()
    {
        _houseService.DetermineAndSetOutsideLights();
    }
    
    private void HandleAway()
    {
        _entities.Switch.ShawnroomMainNetdaemon.TurnOff();
        _entities.Switch.MainroomStateSwitchNetdaemon.TurnOff();
        _entities.Cover.Ratgdov25i0a070cDoor.CloseCover();
        _entities.Lock.HomeConnect620ConnectedSmartLock.Lock();
        _houseService.DetermineAndSetOutsideLights();
    }

    private void HandleSleep()
    {
        _entities.Switch.ShawnroomMainNetdaemon.TurnOff();
    }
}