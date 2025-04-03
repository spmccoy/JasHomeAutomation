using System.Diagnostics.CodeAnalysis;
using NetDaemonApps.Interfaces;

namespace NetDaemonApps.apps.MainRoom.Controls;

/// <summary>
/// Applies room settings for a given state.
/// </summary>
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class MainRoomStateSelect : MqttSelect
{
    private readonly Entities _entities;
    private readonly ILightService _lightService;

    public MainRoomStateSelect(Entities entities, ILightService lightService) 
        : base("MainRoom", "state-select", "Main Room State Select")
    {
        _entities = entities;
        _lightService = lightService;

        AddOption(RoomState.Off, HandleOff);
        AddOption(RoomState.Day, HandleDay);
        AddOption(RoomState.Night, HandleNight);
        AddOption(RoomState.Tv, HandleTv);
        AddOption(RoomState.Dawn, HandleDawn);
        AddOption(RoomState.Dusk, HandleDusk);
    }

    private void HandleDusk()
    {
        _entities.Fan.MainPurifier.SetPercentage(25);
        _entities.Scene.MainBeforeSunset.TurnOn();
    }

    private void HandleDawn()
    {
        _entities.Fan.MainPurifier.SetPercentage(25);
        _entities.Scene.MainBeforeSunrise.TurnOn();
    }

    private void HandleOff()
    {
        _lightService.TurnOffAreaLights(HaArea.MainRoom);
        _entities.Fan.MainPurifier.SetPercentage(100);
        _entities.MediaPlayer.LivingRoomTv.TurnOff();
    }
    
    private void HandleDay()
    {
        _entities.Fan.MainPurifier.SetPercentage(25);
        _lightService.TurnOffAreaLights(HaArea.MainRoom);
    }
    
    private void HandleNight()
    {
        _entities.Fan.MainPurifier.SetPercentage(25);
        _entities.Scene.MainNight.TurnOn();
    }

    private void HandleTv()
    {
        _entities.Fan.MainPurifier.SetPercentage(25);
        _entities.Switch.MainroomStateSwitchNetdaemon.TurnOn();
    }
}