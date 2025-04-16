using Domain.Entities;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.Services;

public class MainRoomService(
    Entities entities, 
    ISunService sunService,
    ILogger<MainRoomService> logger,
    FanEntities fans,
    SceneEntities scenes,
    MediaPlayerEntities players,
    ILightService lightService) 
    : IMainRoomService
{
    public SelectEntity Select => entities.Select.MainroomStateSelectNetdaemon;
    public SwitchEntity Switch => entities.Switch.MainroomStateSwitchNetdaemon;
    
    public void DetermineAndSetRoomState()
    {
        var illumination = sunService.GetCurrentSunState().CurrentSolarIllumination;
        
        switch (illumination)
        {
            case Sun.SolarIllumination.Unknown:
                logger.LogWarning("Current solar illumination is unknown. Switching to 'Off' state.");
                Select.SelectOption(RoomStates.Off.ToString());
                break;
            case Sun.SolarIllumination.Day:
                Select.SelectOption(RoomStates.Day.ToString());
                break;
            case Sun.SolarIllumination.Night:
                Select.SelectOption(RoomStates.Night.ToString());
                break;
            case Sun.SolarIllumination.Dawn:
                Select.SelectOption(RoomStates.Dawn.ToString());
                break;
            case Sun.SolarIllumination.Dusk:
                Select.SelectOption(RoomStates.Dusk.ToString());
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(illumination), $"Unexpected solar illumination: {illumination}");
        }
    }
    
    public void HandleDusk()
    {
        fans.MainPurifier.SetPercentage(25);
        scenes.MainBeforeSunset.TurnOn();
    }

    public void HandleDawn()
    {
        fans.MainPurifier.SetPercentage(25);
        scenes.MainBeforeSunrise.TurnOn();
    }

    public void HandleOff()
    {
        lightService.TurnOffAreaLights(HaArea.MainRoom);
        fans.MainPurifier.SetPercentage(100);
        players.LivingRoomTv.TurnOff();
    }
    
    public void HandleDay()
    {
        fans.MainPurifier.SetPercentage(25);
        lightService.TurnOffAreaLights(HaArea.MainRoom);
    }
    
    public void HandleNight()
    {
        fans.MainPurifier.SetPercentage(25);
        scenes.MainNight.TurnOn();
    }

    public void HandleTv()
    {
        fans.MainPurifier.SetPercentage(25);
    }

    public void HandleSwitch()
    {
        if (AreSwitchAndSelectStatesInSync())
        {
            return;
        }
                
        switch (Switch.State)
        {
            case HaState.On:
                DetermineAndSetRoomState();
                break;

            case HaState.Off:
                Select.SelectOption(RoomStates.Off.ToString());
                break;
        }
    }
    
    public bool AreSwitchAndSelectStatesInSync()
    {
        return (Switch.State == HaState.Off && Select.State == RoomStates.Off.ToString()) ||
               (Switch.State != HaState.Off && Select.State != RoomStates.Off.ToString());
    }
    
    public void SyncSwitchToSelect()
    {
        if (Select.State == RoomStates.Off.ToString())
        {
            Switch.TurnOff();
        }
        else
        {
            Switch.TurnOn();
        }
    }
}