using MqttEntities.Models;
using NetDaemonApps.Extensions;
using NetDaemonApps.Interfaces;
using NetDaemonApps.Models;

namespace NetDaemonApps.Services;

public class HouseService(
    IPersonService personService,
    LightEntities lights,
    SceneEntities scenes,
    LockEntities locks,
    IMainRoomService mainRoomService,
    SelectEntities selects,
    CoverEntities covers,
    ButtonEntities buttons,
    SwitchEntities switches,
    SunEntities suns,
    ClimateEntities thermostats) : IHouseService
{
    public SelectEntity Select => selects.HouseStateNetdaemon;

    public bool HouseSecure => covers.Ratgdov25i0a070cDoor.State == HaState.Closed &&
                               locks.HomeConnect620ConnectedSmartLock.State == HaState.Locked;
    
    public RoomState CurrentRoomState => RoomStates.FromString(Select.State);

    public void DetermineAndSetHouseState()
    {
        if (!personService.AnyoneHome)
        {
            Select.SelectAway();
            return;
        }

        switch (HouseSecure)
        {
            case true:
                Select.SelectHomeSecure();
                return;
            case false:
                Select.SelectHomeUnSecure();
                break;
        }
    }

    public void DetermineAndSetOutsideLights()
    {
        // if it is not dark or past midnight turn off the lights
        if (!suns.Sun.IsDark() || DateTime.Now.Hour < 16)
        {
            lights.PermanentLights.TurnOff();
            return;
        }

        if (CurrentRoomState == RoomStates.HomeUnsecured)
        {
            scenes.GoveeToMqttOneClickDefaultPermanentIntruder.TurnOn();
        }
        else
        {
            scenes.GoveeToMqttOneClickDefaultPermanentDefault.TurnOn();
        }
    }
    
    public void HandleHomeSecure()
    {
        buttons.HouseGarageDoorCloseNetdaemon.Press();
        locks.HomeConnect620ConnectedSmartLock.Lock();
        DetermineAndSetOutsideLights();
        switches.HouseCameraNotificationsNetdaemon.TurnOn();
        thermostats.T6ProZWaveProgrammableThermostat.SetNormal();
    }
    
    public void HandleHomeUnsecured()
    {
        DetermineAndSetOutsideLights();
        switches.HouseCameraNotificationsNetdaemon.TurnOff();
        thermostats.T6ProZWaveProgrammableThermostat.SetNormal();
    }
    
    public void HandleAway()
    {
        switches.ShawnroomStateNetdaemon.TurnOff();
        mainRoomService.Switch.TurnOff();
        buttons.HouseGarageDoorCloseNetdaemon.Press();
        locks.HomeConnect620ConnectedSmartLock.Lock();
        DetermineAndSetOutsideLights();
        switches.HouseCameraNotificationsNetdaemon.TurnOn();
        thermostats.T6ProZWaveProgrammableThermostat.SetNormal();
    }

    public void HandleSleep()
    {
        switches.ShawnroomStateNetdaemon.TurnOff();
        mainRoomService.Switch.TurnOff();
        switches.HouseCameraNotificationsNetdaemon.TurnOn();
        thermostats.T6ProZWaveProgrammableThermostat.SetColder();
    }
}