using Domain.Entities;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.Services;

public class HouseService(
    IPersonService personService, 
    ISunService sunService,
    LightEntities lights,
    SceneEntities scenes,
    LockEntities locks,
    IShawnRoomService shawnRoomService,
    IMainRoomService mainRoomService,
    SelectEntities selects,
    CoverEntities covers) : IHouseService
{
    public SelectEntity Select => selects.HouseStateNetdaemon;

    public bool HouseSecure => covers.Ratgdov25i0a070cDoor.State == HaState.Closed &&
                               locks.HomeConnect620ConnectedSmartLock.State == HaState.Locked;
    
    public RoomState CurrentRoomState => RoomStates.FromString(Select.State);
    
    public void DetermineAndSetHouseState()
    {
        if (personService.IsNoOneHome())
        {
            Select.SelectOption(RoomStates.Away.ToString());
            return;
        }

        switch (HouseSecure)
        {
            case true:
                Select.SelectOption(RoomStates.HomeSecure.ToString());
                return;
            case false:
                Select.SelectOption(RoomStates.HomeUnsecured.ToString());
                break;
        }
    }

    public void DetermineAndSetOutsideLights()
    {
        var now = DateTime.Now;
        // if it is not dark or past midnight turn off the lights
        if (!sunService.IsDark || now > now.Date.AddHours(0))
        {
            lights.PermanentLights.TurnOff();
            return;
        }

        if (CurrentRoomState == RoomStates.HomeUnsecured)
        {
            scenes.GoveeToMqttOneClickDefaultPermanentDefault.TurnOn();
        }
        else
        {
            scenes.GoveeToMqttOneClickDefaultPermanentIntruder.TurnOn();
        }
    }
    
    public void HandleHomeSecure()
    {
        covers.HouseGarageDoorNetdaemon.CloseCover();
        locks.HomeConnect620ConnectedSmartLock.Lock();
        DetermineAndSetOutsideLights();
    }
    
    public void HandleHomeUnsecured()
    {
        DetermineAndSetOutsideLights();
    }
    
    public void HandleAway()
    {
        shawnRoomService.Switch.TurnOff();
        mainRoomService.Switch.TurnOff();
        covers.HouseGarageDoorNetdaemon.CloseCover();
        locks.HomeConnect620ConnectedSmartLock.Lock();
        DetermineAndSetOutsideLights();
    }

    public void HandleSleep()
    {
        shawnRoomService.Switch.TurnOff();
        mainRoomService.Switch.TurnOff();
    }
}