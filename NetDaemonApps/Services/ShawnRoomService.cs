using System.Threading.Tasks;
using MqttEntities.Models;
using NetDaemonApps.Interfaces;
using NetDaemonApps.Models;

namespace NetDaemonApps.Services;

public class ShawnRoomService(
    ILogger<ShawnRoomService> logger,
    SelectEntities selects,
    FanEntities fans,
    SwitchEntities switches,
    SceneEntities scenes,
    ILightService lightService,
    RemoteEntities remotes,
    SunEntities sunEntities) : IShawnRoomService
{
    public SelectEntity Select => selects.ShawnroomStateNetdaemon;
    public SwitchEntity Switch => switches.ShawnroomStateNetdaemon;

    private readonly Sun _sun = new(sunEntities.Sun);
    
    public void DetermineAndSetRoomState()
    {
        var position = _sun.CurrentSolarPosition;
        switch (position)
        {
            case Sun.SolarPosition.Unknown or Sun.SolarPosition.Unavailable:
                logger.LogWarning("Current solar position is unknown. Switching to 'Off' state.");
                selects.ShawnroomStateNetdaemon.SelectOption(RoomStates.Off.ToString());
                break;
            case Sun.SolarPosition.AboveHorizon:
                selects.ShawnroomStateNetdaemon.SelectOption(RoomStates.Day.ToString());
                break;
            case Sun.SolarPosition.BelowHorizon:
                selects.ShawnroomStateNetdaemon.SelectOption(RoomStates.Night.ToString());
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(position), $"Unexpected solar position: {position}");
        }
    }

    public async Task HandleOffAsync()
    {
        fans.ShawnPurifier.SetPercentage(100);
        lightService.TurnOffAreaLights(HaArea.ShawnRoom);
        remotes.ShawnSOfficeTv.TurnOff();
        await Task.Delay(2000);
        switches.ShawnOfficeHueMotionSensorMotionSensorEnabled.TurnOn();
    }
    
    public void HandleDay()
    {
        fans.ShawnPurifier.SetPercentage(33);
        scenes.ShawnSOfficeDay.TurnOn();
    }
    
    public void HandleNight()
    {
        fans.ShawnPurifier.SetPercentage(33);
        scenes.ShawnsOfficeNight.TurnOn();
    }
    
    public void HandleSimRacing()
    {
        Switch.TurnOn();
    }

    public void HandleGaming()
    {
        Switch.TurnOn();
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