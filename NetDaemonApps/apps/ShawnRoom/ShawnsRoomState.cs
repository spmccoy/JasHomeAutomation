using System.Diagnostics.CodeAnalysis;
using System.Threading;
using NetDaemonApps.Interfaces;
using NetDaemonApps.Services;

namespace NetDaemonApps.apps.ShawnRoom;

/// <summary>
/// Applies room settings for a given state.
/// </summary>
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class ShawnsRoomState : MqttSelect
{
    private readonly Entities _entities;
    private readonly ILightService _lightService;
    
    public const string Off = "Off";
    public const string Day = "Day";
    public const string Night = "Night";
    public const string Gaming = "Gaming";
    public const string SimRacing = "Sim Racing";
    
    public ShawnsRoomState(Entities entities, ILightService lightService) 
        : base("ShawnRoom", "state", "Shawn's Room State")
    {
        _entities = entities;
        _lightService = lightService;
        AddOption(Off, HandleOff);
        AddOption(Day, HandleDay);
        AddOption(Night, HandleNight);
        AddOption(Gaming, HandleGaming);
        AddOption(SimRacing, HandleSimRacing);
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
        _entities.Switch.NetdaemonShawnroomMain.TurnOn();
    }

    private void HandleGaming()
    {
        _entities.Switch.NetdaemonShawnroomMain.TurnOn();
    }
}