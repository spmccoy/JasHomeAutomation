using System.Diagnostics.CodeAnalysis;
using System.Threading;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.apps.ShawnRoom.Controls;

/// <summary>
/// Applies room settings for a given state.
/// </summary>
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class ShawnsRoomStateSelect : MqttSelect
{
    private readonly Entities _entities;
    private readonly ILightService _lightService;
    
    public ShawnsRoomStateSelect(Entities entities, ILightService lightService) 
        : base("ShawnRoom", "state", "Shawn's Room State Select")
    {
        _entities = entities;
        _lightService = lightService;
        AddOption(RoomState.Off, HandleOff);
        AddOption(RoomState.Day, HandleDay);
        AddOption(RoomState.Night, HandleNight);
        AddOption(RoomState.Gaming, HandleGaming);
        AddOption(RoomState.SimRacing, HandleSimRacing);
    }
    
    private void HandleOff()
    {
        _entities.Fan.ShawnPurifier.SetPercentage(100);
        _lightService.TurnOffAreaLights(HaArea.ShawnRoom);
        _entities.Remote.ShawnSOfficeTv.TurnOff();
        Thread.Sleep(2000);
        _entities.Switch.ShawnOfficeHueMotionSensorMotionSensorEnabled.TurnOn();
    }
    
    private void HandleDay()
    {
        _entities.Fan.ShawnPurifier.SetPercentage(33);
        _entities.Scene.ShawnSOfficeDay.TurnOn();
    }
    
    private void HandleNight()
    {
        _entities.Fan.ShawnPurifier.SetPercentage(33);
        _entities.Scene.ShawnsOfficeNight.TurnOn();
    }
    
    private void HandleSimRacing()
    {
        _entities.Switch.ShawnroomMainNetdaemon.TurnOn();
    }

    private void HandleGaming()
    {
        _entities.Switch.ShawnroomMainNetdaemon.TurnOn();
    }
}